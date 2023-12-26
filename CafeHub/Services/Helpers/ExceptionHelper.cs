using Microsoft.Extensions.Logging;

namespace CafeHub.Services.Helpers;

public class ExceptionHelper(LoggingHelper exceptionLH) : Exception(exceptionLH.Message)
{
    public readonly LoggingHelper ExceptionLH = exceptionLH;
}
