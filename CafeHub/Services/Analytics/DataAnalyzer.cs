
using CafeHub.Models.Domain;
using CafeHub.Models.ReportProcessing;

namespace CafeHub.Services.Analytics;

public class DataAnalyzer
{
    private readonly object DataAnalyzerLock = new();
    private readonly Queue<TextAnalysis> TextAnalysisQueue = new();
    private int CountAnalizeResults = 0;
    private int ProcessThread = 0;

    public ReportSummary StartAnalytic(Queue<Complaint> complaintSummaries)
    {
        int countComplaints = complaintSummaries.Count;
        Dictionary<string, int> allCommonWords = [];
        string[] noneMergeProperty = ["Id", "IdReport"];

        while (complaintSummaries.Count > 0)
        {
            Complaint complaint = complaintSummaries.Dequeue();
            while (ProcessThread >= 5)
            {
                Thread.Sleep(100);
                continue;
            }
            new Thread(new ParameterizedThreadStart((obj) => { NextAnalizeText(complaint); })).Start();
            ProcessThread++;
        }
        while (ProcessThread > 0)
        {
            Thread.Sleep(100);
            continue;
        }
        ReportSummary baseReport = new()
        {
            MainInfo = new()
            {
                CountMessages = CountAnalizeResults
            }
        };
        while (TextAnalysisQueue.Count > 0)
        {
            TextAnalysis report = TextAnalysisQueue.Dequeue();
            MergerReports(baseReport.AggregatePartsSpeech, report.PartsSpeechMessage, noneMergeProperty, 0);
            MergerReports(baseReport.AverageTonality, report.TonalityMessage, noneMergeProperty, 0f);
            foreach (var entry in report.CommonWordsMessage)
            {
                if (allCommonWords.TryGetValue(entry.Key, out int value))
                    allCommonWords[entry.Key] = value + entry.Value;
                else
                    allCommonWords[entry.Key] = entry.Value;
            }
        }
        var topWords = allCommonWords.OrderByDescending(kv => kv.Value).Take(5);
        foreach (var word in topWords)
            baseReport.AggregateCommonWords.Add(new CommonWord()
            {
                Word = word.Key,
                NumberRepetitions = word.Value,
            });

        if (CountAnalizeResults > 0)
        {
            baseReport.AverageTonality.Positive /= CountAnalizeResults;
            baseReport.AverageTonality.Negative /= CountAnalizeResults;
            baseReport.AverageTonality.Median /= CountAnalizeResults;
        }
        return baseReport;
    }

    private async void NextAnalizeText(Complaint complaint)
    {
        TextAnalysis? reportMessage = null;
        if (!string.IsNullOrEmpty(complaint.Description))
        {
            TextAnalyzer messageAnalyzer = new(complaint.Description);
            reportMessage = await messageAnalyzer.StartAnalyticMessage();
        }
        lock (DataAnalyzerLock)
        {
            if (reportMessage != null)
            {
                reportMessage.Location = complaint.Location;
                reportMessage.Сategory = complaint.Сategory;
                TextAnalysisQueue.Enqueue(reportMessage);
                CountAnalizeResults++;
            }
            ProcessThread--;
        }
    }

    private static void MergerReports<T, TSource>(T firstMergeObject, T secondMergeObject, string[] propNotMerge, TSource defaultValue)
    {
        var allProperties = typeof(T).GetProperties()
            .Where(prop => !propNotMerge.Contains(prop.Name));

        foreach (var sourceProperty in allProperties)
        {
            if (sourceProperty.GetValue(firstMergeObject) is TSource firstObjectPropValue && sourceProperty.GetValue(secondMergeObject) is TSource secondObjectPropValue)
            {
                var newValue = AddValues(firstObjectPropValue, secondObjectPropValue, defaultValue);
                sourceProperty.SetValue(firstMergeObject, newValue);
            }
        }
    }

    private static TSource AddValues<TSource>(TSource value1, TSource value2, TSource defaultValue)
    {
        if (EqualityComparer<TSource>.Default.Equals(defaultValue, default))
        {
            dynamic dynamicValue1 = value1!;
            dynamic dynamicValue2 = value2!;
            return dynamicValue1 + dynamicValue2;
        }
        else
            return value1;
    }
}
