
using CHDesktop.Models.Enums;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using System.Net;
using CHDesktop.Models.Domain;
using CHDesktop.Services.Helpers;
using System.Collections.Generic;
using CHDesktop.Models.ReportProcessing;

namespace CHDesktop.Services.Server;

internal class UserHttp : MainHttp
{
    public async Task<(LoggingHelper, User?)> GetUserInfo()
    {
        string uri = $"https://{IP}:{Port}/user/userinfo";
        ResponseModel responseModel = await GetRequest(uri);
        if (responseModel.LoggingAnswer.Type == TypeMessage.Problem)
            responseModel.LoggingAnswer.Type = TypeMessage.NoneAuthorize;
        User? user = !string.IsNullOrEmpty(responseModel.RequestObject) ? JsonSerializer.Deserialize<User>(responseModel.RequestObject) : null;
        return (responseModel.LoggingAnswer, user);
    }


    public async Task<(LoggingHelper, User?)> LogInUser(string login, string password)
    {
        string uri = $"https://{IP}:{Port}/user/login/{login}/{password}";
        ResponseModel responseModel = await GetRequest(uri);
        User? user = !string.IsNullOrEmpty(responseModel.RequestObject) ? JsonSerializer.Deserialize<User>(responseModel.RequestObject) : null;
        return (responseModel.LoggingAnswer, user);
    }


    public async Task<(LoggingHelper, User?)> SignUpUser(User user)
    {
        string uri = $"https://{IP}:{Port}/user/signup";
        string jsonRequestBody = JsonSerializer.Serialize(user);
        ResponseModel responseModel = await PostRequest(uri, jsonRequestBody);
        User? addUser = !string.IsNullOrEmpty(responseModel.RequestObject) ? JsonSerializer.Deserialize<User>(responseModel.RequestObject) : null;
        return (responseModel.LoggingAnswer, addUser);
    }

    public async Task<(LoggingHelper, User?)> UpdateUserInfo(User updateUser)
    {
        string uri = $"https://{IP}:{Port}/user/updateUser";
        string jsonRequestBody = JsonSerializer.Serialize(updateUser);
        ResponseModel responseModel = await PutRequest(uri, jsonRequestBody);
        User? newUser = !string.IsNullOrEmpty(responseModel.RequestObject) ? JsonSerializer.Deserialize<User>(responseModel.RequestObject) : null;
        return (responseModel.LoggingAnswer, newUser);
    }

    public async Task<LoggingHelper> UpdatePassword(UpdatePasswordModel updatePassword)
    {
        string uri = $"https://{IP}:{Port}/user/updatePassword";
        string jsonRequestBody = JsonSerializer.Serialize(updatePassword);
        ResponseModel responseModel = await PutRequest(uri, jsonRequestBody);
        return responseModel.LoggingAnswer;
    }

    public async Task<LoggingHelper> DeleteUser()
    {
        string uri = $"https://{IP}:{Port}/user/deleteUser";
        ResponseModel responseModel = await DeleteRequest(uri);
        return responseModel.LoggingAnswer;
    }

    public async Task<(LoggingHelper, List<ComplaintSummary>?)> GetComplaints()
    {
        string uri = $"https://{IP}:{Port}/user/сomplaints";
        ResponseModel responseModel = await GetRequest(uri);
        List<ComplaintSummary>? complaints = !string.IsNullOrEmpty(responseModel.RequestObject) ? JsonSerializer.Deserialize<List<ComplaintSummary>>(responseModel.RequestObject) : null;
        return (responseModel.LoggingAnswer, complaints);
    }

    public async Task<LoggingHelper> AddComplaint(ComplaintSummary complaintSummary)
    {
        string uri = $"https://{IP}:{Port}/user/addComplaint";
        string jsonRequestBody = JsonSerializer.Serialize(complaintSummary);
        ResponseModel responseModel = await PostRequest(uri, jsonRequestBody);
        return responseModel.LoggingAnswer;
    }

    public async Task<LoggingHelper> DeleteComplaint(int idComplaint)
    {
        string uri = $"https://{IP}:{Port}/user/deleteComplaint/{idComplaint}";
        ResponseModel responseModel = await DeleteRequest(uri);
        return responseModel.LoggingAnswer;
    }

    public async Task<LoggingHelper> EditStatusComplaint(int idComplaint, StatusComplaint newStatus)
    {
        string uri = $"https://{IP}:{Port}/user/editStatus/{idComplaint}/{newStatus}";
        ResponseModel responseModel = await PutRequest(uri, "");
        return responseModel.LoggingAnswer;
    }
}
