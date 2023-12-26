
using CafeHub.Models.Domain;
using CafeHub.Models.Enums;
using Microsoft.Extensions.Logging;
using System.Runtime.InteropServices;

namespace CafeHub.Services.Helpers;

public class LoggingHelper
{
    public TypeMessage Type { get; set; } = TypeMessage.Ordinary;
    public DangerLevel DangerLevel { get; set; } = DangerLevel.Oke;
    public string Message { get; set; } = "Okey";
    public string? Description { get; set; } = string.Empty;
    internal object? SomeObject { get; set; }

    public LoggingHelper() { }

    public LoggingHelper(TypeMessage typeLevel, DangerLevel dangerLevel, string messange, object? someObject = null)
    {
        Type = typeLevel;
        DangerLevel = dangerLevel;
        Message = messange;
        SomeObject = someObject;
    }

    public LoggingHelper(LoggingHelper logginMain)
    {
        Type = logginMain.Type;
        DangerLevel = logginMain.DangerLevel;
        Message = logginMain.Message;
        Description = logginMain.Description;
        SomeObject = logginMain.SomeObject;
    }

    public LoggingHelper Clone()
    {
        return new LoggingHelper(Type, DangerLevel, Message, SomeObject)
        {
            Description = Description
        };
    }
}
