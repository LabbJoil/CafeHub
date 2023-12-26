using CafeHub.Models.Domain;
using CafeHub.Models.Enums;

namespace CafeHub.Models.ReportProcessing;

public class TextAnalysis
{
    public Tonality? TonalityMessage { get; set; }
    public PartsSpeech? PartsSpeechMessage { get; set; }
    public Dictionary<string, int> CommonWordsMessage = [];
    public LocationCafe Location { get; set; }
    public СategoryComplaint Сategory {  get; set; }
}
