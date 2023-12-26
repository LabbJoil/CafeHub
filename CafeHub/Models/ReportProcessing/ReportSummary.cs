
using CafeHub.Models.Domain;
using CafeHub.Models.Enums;

namespace CafeHub.Models.ReportProcessing;

public class ReportSummary
{
    public Report MainInfo { get; set; } = new();
    public Tonality AverageTonality { get; set; } = new();
    public PartsSpeech AggregatePartsSpeech { get; set; } = new();
    public List<CommonWord> AggregateCommonWords { get; set; } = [];
    public Dictionary<LocationCafe, int> AggregateLocationCafe = new() 
    {
        {LocationCafe.KuznechnyLane3, 0},
        {LocationCafe.NovoroshchinskayaStreet4, 0},
        {LocationCafe.KirpichnyLane8, 0},
        {LocationCafe.FedorAbramovStreet16k1, 0},
        {LocationCafe.KonstantinovskyProspekt23, 0},
    };
    public Dictionary<СategoryComplaint, int> AggregateСategoryComplaint = new()
    {
        { СategoryComplaint.Staff, 0 },
        { СategoryComplaint.Food, 0 },
        { СategoryComplaint.Situation, 0 }
    };
}
