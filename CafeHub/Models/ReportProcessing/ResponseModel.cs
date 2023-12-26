using CafeHub.Services.Helpers;

namespace CafeHub.Models.ReportProcessing;

public class ResponseModel(string? requestObject, LoggingHelper loggingAnswer, string? token)
{
    public string? RequestObject { get; set; } = requestObject;
    public LoggingHelper? LoggingAnswer { get; set; } = loggingAnswer;
    public string? Token { get; set; } = token;
}
