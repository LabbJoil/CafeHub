

using CHDesktop.Models.Domain;
using CHDesktop.Models.Enums;

namespace CHDesktop.Models.ReportProcessing;

public class ReportSummary
{
    public Report MainInfo { get; set; } = new();
    public Tonality AverageTonality { get; set; } = new();
    public PartsSpeech AggregatePartsSpeech { get; set; } = new();
    public List<CommonWord> AggregateCommonWords { get; set; } = [];
    public Dictionary<LocationCafe, int> AggregateLocationCafe = [];
    public Dictionary<СategoryComplaint, int> AggregateСategoryComplaint = [];
}
