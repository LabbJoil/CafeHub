
using CafeHub.Models.Domain;
using CafeHub.Models.Enums;
using CafeHub.Models.ReportProcessing;
using CafeHub.Services.Background;
using CafeHub.Services.DataAccess;
using CafeHub.Services.Entities;
using CafeHub.Services.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;

namespace CafeHub.Controllers;

[Route("report/")]
[ApiController]
[Authorize]
public class ReportController(ContextDB ContextDB, ILogger<UserController> logger) : ControllerBase
{
    private readonly ReportHelper ReportDBHelper = new(ContextDB);
    private readonly ILogger<UserController> Logger = logger;

    [HttpPost("doAnalytics")]
    public IActionResult DoAnalyticsController([FromBody] List<int> dialogModel)
    {
        int idUser = -1;
        string token = "";
        try
        {
            (idUser, UserRole role) = AuthOptions.GetUserIdFromToken(User);
            Logger.LogInformation("|{date}|\t|User (id - {idUser})|\t|Requested create analytics", DateTime.Now, idUser);
            token = new JwtSecurityTokenHandler().WriteToken(AuthOptions.NewToken(User.Claims));
            BackgroundAnalyticsProcessor.AddDialog(idUser, dialogModel);
            Logger.LogInformation("|{date}|\t|User (id - {idUser})|\t|Message analytics added to the queue", DateTime.Now, idUser);
            return Ok(new ResponseModel(
                null,
                new LoggingHelper(TypeMessage.Ordinary, DangerLevel.Oke, "Message analytics added to the queue"),
                token)
            );
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "|{date}|\t|User (id - {idUser})|", DateTime.Now, idUser);
            LoggingHelper loggingHelper = ex is ExceptionHelper exh
                ? exh.ExceptionLH
                : new LoggingHelper(TypeMessage.Problem, DangerLevel.Error, ex.Message);
            return BadRequest(new ResponseModel(null, loggingHelper, token));
        }
    }

    [HttpGet("reports")]
    public IActionResult GetReportsController()
    {
        int idUser = -1;
        string token = "";
        try
        {
            (idUser, UserRole role) = AuthOptions.GetUserIdFromToken(User);
            Logger.LogInformation("|{date}|\t|User (id - {idUser})|\t|Requested to get reports", DateTime.Now, idUser);
            token = new JwtSecurityTokenHandler().WriteToken(AuthOptions.NewToken(User.Claims));
            List<Report> reports = ReportDBHelper.GetReportsUser(idUser);
            Logger.LogInformation("|{date}|\t|User (id - {idUser})|\t|Reports have been received", DateTime.Now, idUser);
            return Ok(new ResponseModel(
                JsonSerializer.Serialize(reports),
                new LoggingHelper(TypeMessage.Ordinary, DangerLevel.Oke, "List reports"),
                token)
            );
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "|{date}|\t|User (id - {idUser})|", DateTime.Now, idUser);
            LoggingHelper loggingHelper = ex is ExceptionHelper exh
                ? exh.ExceptionLH
                : new LoggingHelper(TypeMessage.Problem, DangerLevel.Error, ex.Message);
            return BadRequest(new ResponseModel(null, loggingHelper, token));
        }
    }

    [HttpGet("reportId/{idReport}")]
    public IActionResult GetReportByIdController(int idReport)
    {
        int idUser = -1;
        string token = "";
        try
        {
            (idUser, UserRole role) = AuthOptions.GetUserIdFromToken(User);
            Logger.LogInformation("|{date}|\t|User (id - {idUser})|\t|Requested get report by id - {idReport}", DateTime.Now, idUser, idReport);
            token = new JwtSecurityTokenHandler().WriteToken(AuthOptions.NewToken(User.Claims));
            ReportSummary report = ReportDBHelper.GetReportById(idUser, idReport);
            Logger.LogInformation("|{date}|\t|User (id - {idUser})|\t|Requested create analytics", DateTime.Now, idUser);
            return Ok(new ResponseModel(
                JsonSerializer.Serialize(report),
                new LoggingHelper(TypeMessage.Ordinary, DangerLevel.Oke, "Report"),
                token)
            );
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "|{date}|\t|User (id - {idUser})|", DateTime.Now, idUser);
            LoggingHelper loggingHelper = ex is ExceptionHelper exh
                ? exh.ExceptionLH
                : new LoggingHelper(TypeMessage.Problem, DangerLevel.Error, ex.Message);
            return BadRequest(new ResponseModel(null, loggingHelper, token));
        }
    }

    [HttpDelete("deleteReport/{idReport}")]
    public async Task<IActionResult> DeleteReportByIdController(int idReport)
    {
        int idUser = -1;
        string token = "";
        try
        {
            (idUser, UserRole role) = AuthOptions.GetUserIdFromToken(User);
            Logger.LogInformation("|{date}|\t|User (id - {idUser})|\t|Requested delete report", DateTime.Now, idUser);
            token = new JwtSecurityTokenHandler().WriteToken(AuthOptions.NewToken(User.Claims));
            await ReportDBHelper.DeleteReport(idUser, idReport);
            Logger.LogInformation("|{date}|\t|User (id - {idUser})|\t|The report has been deleted", DateTime.Now, idUser);
            return Ok(new ResponseModel(
                null,
               new(TypeMessage.Ordinary, DangerLevel.Oke, "You delete the report"),
                token)
            );
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "|{date}|\t|User (id - {idUser})|", DateTime.Now, idUser);
            LoggingHelper loggingHelper = ex is ExceptionHelper exh
                ? exh.ExceptionLH
                : new LoggingHelper(TypeMessage.Problem, DangerLevel.Error, ex.Message);
            return BadRequest(new ResponseModel(null, loggingHelper, token));
        }
    }
}
