
using CafeHub.Models.Domain;
using CafeHub.Models.Enums;
using CafeHub.Models.ReportProcessing;
using CafeHub.Services.Entities;
using CafeHub.Services.Helpers;

namespace CafeHub.Services.DataAccess;

public class ReportHelper(ContextDB contextDB)
{
    private readonly ContextDB Database = contextDB;

    public List<Report> GetReportsUser(int idUser)
    {
        List<Report> reports = [.. Database.Reports.Where(report => report.IdUser == idUser)];
        return reports;
    }

    public ReportSummary GetReportById(int idUser, int idReport)
    {
        ReportSummary analyticReport = new()
        {
            MainInfo = Database.Reports.Where(report => report.IdUser == idUser && report.Id == idReport).FirstOrDefault()
                ?? throw new ExceptionHelper(new LoggingHelper(TypeMessage.Problem, DangerLevel.Wanted, "The report is missing")),
            AverageTonality = Database.Tonalitys.Where(tonality => tonality.IdReport == idReport).FirstOrDefault() ?? new(),
            AggregatePartsSpeech = Database.PartsSpeechs.Where(tonality => tonality.IdReport == idReport).FirstOrDefault() ?? new(),
            AggregateCommonWords = [.. Database.CommonWords.Where(cw => cw.IdReport == idReport)],

            AggregateLocationCafe = { 
                { LocationCafe.KirpichnyLane8, Database.Complaints.Count(c1 => c1.Location == LocationCafe.KirpichnyLane8) },
                { LocationCafe.NovoroshchinskayaStreet4, Database.Complaints.Count(c1 => c1.Location == LocationCafe.NovoroshchinskayaStreet4) },
                { LocationCafe.FedorAbramovStreet16k1, Database.Complaints.Count(c1 => c1.Location == LocationCafe.FedorAbramovStreet16k1) },
                { LocationCafe.KonstantinovskyProspekt23, Database.Complaints.Count(c1 => c1.Location == LocationCafe.KonstantinovskyProspekt23) },
                { LocationCafe.KuznechnyLane3, Database.Complaints.Count(c1 => c1.Location == LocationCafe.KuznechnyLane3) },
            },
            
            AggregateСategoryComplaint = { 
                { СategoryComplaint.Staff, Database.Complaints.Count(c1 => c1.Сategory == СategoryComplaint.Staff) },
                { СategoryComplaint.Food, Database.Complaints.Count(c1 => c1.Сategory == СategoryComplaint.Food) },
                { СategoryComplaint.Situation, Database.Complaints.Count(c1 => c1.Сategory == СategoryComplaint.Situation) }
            }
        };
        return analyticReport;
    }

    public async Task DeleteReport(int idUser, int IdReport)
    {
        var deleteReport = Database.Reports.Where(report => report.Id == IdReport && report.IdUser == idUser).FirstOrDefault()
            ?? throw new ExceptionHelper(new LoggingHelper(TypeMessage.Problem, DangerLevel.Wanted, "Couldn't find this report"));
        Database.Reports.Remove(deleteReport);
        await Database.SaveChangesAsync();
    }
}
