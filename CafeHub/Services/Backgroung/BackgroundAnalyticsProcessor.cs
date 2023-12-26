
using CafeHub.Models.ReportProcessing;
using CafeHub.Services.Analytics;
using CafeHub.Services.Entities;
using CafeHub.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace CafeHub.Services.Background;

public class BackgroundAnalyticsProcessor(IServiceScopeFactory scopeFactory) : BackgroundService
{
    private readonly IServiceScopeFactory ScopeFactory = scopeFactory;
    private static readonly Queue<(int, List<int>)> AnaliticDialogs = new();

    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = ScopeFactory.CreateScope();
            ContextDB Database = scope.ServiceProvider.GetRequiredService<ContextDB>();

            while (AnaliticDialogs.Count > 0)
            {
                try
                {
                    (int idUser, List<int> idsComplaints) = AnaliticDialogs.Dequeue();
                    User? user = Database.Users.Find(idUser);
                    if (user == null || user.Role != Models.Enums.UserRole.Admin)
                        continue;

                    Queue<Complaint> complaintSummaries = [];
                    foreach (int id in idsComplaints)
                    {
                        if(await Database.Complaints.FindAsync([ id ], cancellationToken: stoppingToken) is Complaint complaint)
                            complaintSummaries.Enqueue(complaint);
                    }
                    
                    ReportSummary newAnalitycsReport = new DataAnalyzer().StartAnalytic(complaintSummaries);
                    newAnalitycsReport.MainInfo.CreateDate = DateTime.UtcNow;
                    newAnalitycsReport.MainInfo.IdUser = idUser;
                    newAnalitycsReport.MainInfo.Name = $"{user.FirstName} {user.LastName}";

                    var addedReport = Database.Reports.Add(newAnalitycsReport.MainInfo);
                    await Database.SaveChangesAsync(stoppingToken);

                    int reportId = addedReport.Entity.Id;
                    newAnalitycsReport.AverageTonality.IdReport = reportId;
                    newAnalitycsReport.AggregatePartsSpeech.IdReport = reportId;

                    foreach (var nextWord in newAnalitycsReport.AggregateCommonWords)
                    {
                        nextWord.IdReport = reportId;
                        Database.CommonWords.Add(nextWord);
                    }
                    Database.Tonalitys.Add(newAnalitycsReport.AverageTonality);
                    Database.PartsSpeechs.Add(newAnalitycsReport.AggregatePartsSpeech);
                    await Database.SaveChangesAsync(stoppingToken);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            await Task.Delay(100, stoppingToken);
        }
    }

    public static void AddDialog(int idUser, List<int> idsComplaints)
        => AnaliticDialogs.Enqueue((idUser, idsComplaints));
}
