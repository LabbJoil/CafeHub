
using CafeHub.Models.Domain;
using CafeHub.Models.Enums;

namespace CafeHub.Models.ReportProcessing;

public class ReportSummary
{
    public Report MainInfo { get; set; } = new();
    public Tonality AverageTonality { get; set; } = new();
    public PartsSpeech AggregatePartsSpeech { get; set; } = new();
    public List<CommonWord> AggregateCommonWords { get; set; } = [];
    public Dictionary<LocationCafe, int> AggregateLocationCafe { get; set; } = [];
    public Dictionary<СategoryComplaint, int> AggregateСategoryComplaint { get; set; }  = [];
}
