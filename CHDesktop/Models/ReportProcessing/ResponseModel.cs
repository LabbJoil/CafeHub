
using CHDesktop.Services.Helpers;
using System.Text.Json.Serialization;

namespace CHDesktop.Models.ReportProcessing;

public class ResponseModel
{
    [JsonPropertyName("requestObject")]
    public string? RequestObject { get; set; }
    [JsonPropertyName("loggingAnswer")]
    public LoggingHelper LoggingAnswer { get; set; } = new();
    [JsonPropertyName("token")]
    public string? Token { get; set; }

    public ResponseModel() { }
    public ResponseModel(ResponseModel responseModel)
    {
        LoggingAnswer = responseModel.LoggingAnswer;
        Token = responseModel.Token;
    }
}
