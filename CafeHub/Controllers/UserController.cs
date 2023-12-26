
using CafeHub.Models.Domain;
using CafeHub.Models.Entities;
using CafeHub.Models.Enums;
using CafeHub.Models.ReportProcessing;
using CafeHub.Services.DataAccess;
using CafeHub.Services.Entities;
using CafeHub.Services.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;

namespace CafeHub.Controllers;

[ApiController]
[Route("[controller]/")]
public class UserController(ContextDB ContextDB, ILogger<UserController> logger) : ControllerBase
{
    private readonly UserHelper UserDBHelper = new(ContextDB);
    private readonly ILogger<UserController> Logger = logger;

    [HttpGet("login/{email}/{password}")]
    public IActionResult LogInUserController(string email, string password)
    {
        try
        {
            Logger.LogInformation("|{date}|\t|Requested to login", DateTime.Now);
            User authorizeUser = UserDBHelper.AuthorizeUsers(email, password);
            var jwt = AuthOptions.NewToken(
                new List<Claim>
                {
                    new(ClaimTypes.Sid, authorizeUser.Id.ToString()),
                    new(ClaimTypes.Role, authorizeUser.Role.ToString())
                }
             );
            Logger.LogInformation("|{date}|\t|User (id - {idUser})|\t|The user has successfully logged in", DateTime.Now, authorizeUser.Id);
            return Ok(new ResponseModel(
                JsonSerializer.Serialize(authorizeUser),
                new LoggingHelper(TypeMessage.Authorize, DangerLevel.Oke, "Good authorize"),
                new JwtSecurityTokenHandler().WriteToken(jwt))
                );
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "|{date}|", DateTime.Now);
            LoggingHelper loggingHelper = ex is ExceptionHelper exh
                ? exh.ExceptionLH
                : new LoggingHelper(TypeMessage.Problem, DangerLevel.Error, ex.Message);
            return BadRequest(new ResponseModel(null, loggingHelper, null));
        }
    }


    [HttpPost("signup")]
    public async Task<IActionResult> SignUpUserController([FromBody] User userModel)
    {
        try
        {
            Logger.LogInformation("|{date}|\t|Requested to signup", DateTime.Now);
            User authorizeUser = await UserDBHelper.SetUser(userModel);
            var jwt = AuthOptions.NewToken(
                new List<Claim>
                {
                    new(ClaimTypes.Sid, authorizeUser.Id.ToString()),
                    new(ClaimTypes.Role, authorizeUser.Role.ToString())
                }
             );
            Logger.LogInformation("|{date}|\t|User (id - {idUser})|\t|The user has successfully signup", DateTime.Now, authorizeUser.Id);
            return Ok(new ResponseModel(
                JsonSerializer.Serialize(authorizeUser),
                new LoggingHelper(TypeMessage.Authorize, DangerLevel.Oke, "Good Sing up"),
                new JwtSecurityTokenHandler().WriteToken(jwt))
                );
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "|{date}|", DateTime.Now);
            LoggingHelper loggingHelper = ex is ExceptionHelper exh
                ? exh.ExceptionLH
                : new LoggingHelper(TypeMessage.Problem, DangerLevel.Error, ex.Message);
            return BadRequest(new ResponseModel(null, loggingHelper, null));
        }
    }

    [HttpGet("userInfo")]
    [Authorize]
    public IActionResult GetUserInfoController()
    {
        int idUser = -1;
        try
        {
            (idUser, UserRole role) = AuthOptions.GetUserIdFromToken(User);
            Logger.LogInformation("|{date}|\t|User (id - {idUser})|\t|Requested information about himself", DateTime.Now, idUser);
            var jwt = AuthOptions.NewToken(User.Claims);
            User user = UserDBHelper.GetUserInfo(idUser);
            Logger.LogInformation("|{date}|\t|User (id - {idUser})|\t|User data has been received", DateTime.Now, idUser);
            return Ok(new ResponseModel(
                JsonSerializer.Serialize(user),
                new(TypeMessage.Authorize, DangerLevel.Oke, "Successful"),
                new JwtSecurityTokenHandler().WriteToken(jwt)));
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "|{date}|\t|User (id - {idUser})|", DateTime.Now, idUser);
            LoggingHelper loggingHelper = ex is ExceptionHelper exh
                ? exh.ExceptionLH
                : new LoggingHelper(TypeMessage.Problem, DangerLevel.Error, ex.Message);
            return BadRequest(new ResponseModel(null, loggingHelper, null));
        }
    }

    [HttpPut("updateUser")]
    [Authorize]
    public async Task<IActionResult> UpdateUserController([FromBody] UpdateUserModel userModel)
    {
        int idUser = -1;
        string token = "";
        try
        {
            (idUser, UserRole role) = AuthOptions.GetUserIdFromToken(User);
            Logger.LogInformation("|{date}|\t|User (id - {idUser})|\t|Requested to update information", DateTime.Now, idUser);
            token = new JwtSecurityTokenHandler().WriteToken(AuthOptions.NewToken(User.Claims));
            User user = await UserDBHelper.UpdateUser(idUser, userModel);
            Logger.LogInformation("|{date}|\t|User (id - {idUser})|\t|The information was updated", DateTime.Now, idUser);
            return Ok(new ResponseModel(
                JsonSerializer.Serialize(user),
                new LoggingHelper(TypeMessage.Ordinary, DangerLevel.Oke, "You have changed your profile"),
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

    [HttpPut("updatePassword")]
    [Authorize]
    public async Task<IActionResult> UpdatePasswordController([FromBody] UpdatePasswordModel userModel)
    {
        int idUser = -1;
        string token = "";
        try
        {
            (idUser, UserRole role) = AuthOptions.GetUserIdFromToken(User);
            Logger.LogInformation("|{date}|\t|User (id - {idUser})|\t|Requested to update password", DateTime.Now, idUser);
            token = new JwtSecurityTokenHandler().WriteToken(AuthOptions.NewToken(User.Claims));
            await UserDBHelper.UpdatePassword(idUser, userModel);
            Logger.LogInformation("|{date}|\t|User (id - {idUser})|\t|The password was revealed", DateTime.Now, idUser);
            return Ok(new ResponseModel(
                null,
                new(TypeMessage.Ordinary, DangerLevel.Oke, "You update the password"),
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

    [HttpPut("updateIcon")]
    [Authorize]
    public async Task<IActionResult> UpdateIconController([FromBody] byte[] iconBytes)
    {
        int idUser = -1;
        string token = "";
        try
        {
            (idUser, UserRole role) = AuthOptions.GetUserIdFromToken(User);
            Logger.LogInformation("|{date}|\t|User (id - {idUser})|\t|Requested to update personal icon", DateTime.Now, idUser);
            token = new JwtSecurityTokenHandler().WriteToken(AuthOptions.NewToken(User.Claims));
            await UserDBHelper.UpdateIcon(idUser, iconBytes);
            Logger.LogInformation("|{date}|\t|User (id - {idUser})|\t|The icon was updated", DateTime.Now, idUser);
            return Ok(new ResponseModel(
                null,
                new(TypeMessage.Ordinary, DangerLevel.Oke, "You update the icon"),
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

    [HttpDelete("deleteUser")]
    [Authorize]
    public async Task<IActionResult> DeleteUserController()
    {
        int idUser = -1;
        string token = "";
        try
        {
            Logger.LogInformation("|{date}|\t|User (id - {idUser})|\t|Request to delete a user", DateTime.Now, idUser);
            (idUser, UserRole role) = AuthOptions.GetUserIdFromToken(User);
            if (role == UserRole.Admin)
                throw new ExceptionHelper(new LoggingHelper(TypeMessage.Ordinary, DangerLevel.Wanted, "You don't have access rights"));
            token = new JwtSecurityTokenHandler().WriteToken(AuthOptions.NewToken(User.Claims));
            await UserDBHelper.DeleteUser(idUser);
            Logger.LogInformation("|{date}|\t|User (id - {idUser})|\t|The user has been deleted", DateTime.Now, idUser);
            return Ok(new ResponseModel(
                null,
               new(TypeMessage.Ordinary, DangerLevel.Oke, "You delete the user"),
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

    [HttpGet("сomplaints")]
    [Authorize]
    public async Task<IActionResult> GetReportsController()
    {
        int idUser = -1;
        string token = "";
        try
        {
            (idUser, UserRole role) = AuthOptions.GetUserIdFromToken(User);
            Logger.LogInformation("|{date}|\t|User (id - {idUser})|\t|Requested to get сomplaints", DateTime.Now, idUser);
            token = new JwtSecurityTokenHandler().WriteToken(AuthOptions.NewToken(User.Claims));
            List<ComplaintSummary> complaints = await UserDBHelper.GetComplaints(role, idUser);
            Logger.LogInformation("|{date}|\t|User (id - {idUser})|\t|You have received the сomplaints", DateTime.Now, idUser);
            return Ok(new ResponseModel(
                JsonSerializer.Serialize(complaints),
                new LoggingHelper(TypeMessage.Ordinary, DangerLevel.Oke, "List сomplaints"),
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

    [HttpGet("complaintId/{idComplaint}")]
    [Authorize]
    public async Task<IActionResult> GetReportByIdController(int idComplaint)
    {
        int idUser = -1;
        string token = "";
        try
        {
            (idUser, UserRole role) = AuthOptions.GetUserIdFromToken(User);
            Logger.LogInformation("|{date}|\t|User (id - {idUser})|\t|Requested get complaint by id - {idСomplaint}", DateTime.Now, idUser, idComplaint);
            token = new JwtSecurityTokenHandler().WriteToken(AuthOptions.NewToken(User.Claims));
            ComplaintSummary complaintSummary = await UserDBHelper.GetComplaintById(role, idComplaint, idUser);
            Logger.LogInformation("|{date}|\t|User (id - {idUser})|\t|You have received the complaint", DateTime.Now, idUser);
            return Ok(new ResponseModel(
                JsonSerializer.Serialize(complaintSummary),
                new LoggingHelper(TypeMessage.Ordinary, DangerLevel.Oke, "Сomplaint"),
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

    [HttpPost("addComplaint")]
    [Authorize]
    public async Task<IActionResult> AddComplaintController([FromBody] ComplaintSummary complaintSummary)
    {
        int idUser = -1;
        string token = "";
        try
        {
            (idUser, UserRole role) = AuthOptions.GetUserIdFromToken(User);
            Logger.LogInformation("|{date}|\t|User (id - {idUser})|\t|Requested to add complaint", DateTime.Now, idUser);
            token = new JwtSecurityTokenHandler().WriteToken(AuthOptions.NewToken(User.Claims));
            await UserDBHelper.SetComplaint(idUser, complaintSummary);
            Logger.LogInformation("|{date}|\t|User (id - {idUser})|\t|You have added complaint", DateTime.Now, idUser);
            return Ok(new ResponseModel(
                null,
                new LoggingHelper(TypeMessage.Authorize, DangerLevel.Oke, "You have added complaint"),
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

    [HttpDelete("deleteComplaint/{idComplaint}")]
    [Authorize]
    public async Task<IActionResult> DeleteComplaintController(int idComplaint)
    {
        int idUser = -1;
        string token = "";
        try
        {
            (idUser, UserRole role) = AuthOptions.GetUserIdFromToken(User);
            Logger.LogInformation("|{date}|\t|User (id - {idUser})|\t|Requested to delete complaint", DateTime.Now, idUser);
            token = new JwtSecurityTokenHandler().WriteToken(AuthOptions.NewToken(User.Claims));
            await UserDBHelper.DeleteComplaint(role, idUser, idComplaint);
            Logger.LogInformation("|{date}|\t|User (id - {idUser})|\t|You have deleted complaint", DateTime.Now, idUser);
            return Ok(new ResponseModel(
                null,
                new LoggingHelper(TypeMessage.Authorize, DangerLevel.Oke, "You have deleted complaint"),
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

    [HttpPut("editStatus/{idComplaint}/{newStatus}")]
    [Authorize]
    public async Task<IActionResult> EditStatusComplaintController(int idComplaint, StatusComplaint newStatus)
    {
        int idUser = -1;
        string token = "";
        try
        {
            (idUser, UserRole role) = AuthOptions.GetUserIdFromToken(User);
            if (role == UserRole.User)
                throw new ExceptionHelper(new LoggingHelper(TypeMessage.Ordinary, DangerLevel.Wanted, "You don't have access rights"));
            Logger.LogInformation("|{date}|\t|User (id - {idUser})|\t|Requested to delete complaint", DateTime.Now, idUser);
            token = new JwtSecurityTokenHandler().WriteToken(AuthOptions.NewToken(User.Claims));
            await UserDBHelper.EditStatusComplaint(idComplaint, newStatus);
            Logger.LogInformation("|{date}|\t|User (id - {idUser})|\t|You have deleted complaint", DateTime.Now, idUser);
            return Ok(new ResponseModel(
                null,
                new LoggingHelper(TypeMessage.Authorize, DangerLevel.Oke, "You have deleted complaint"),
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
