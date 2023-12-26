
using CHDesktop.Models.Domain;
using CHDesktop.Models.ReportProcessing;
using CHDesktop.Services.Helpers;
using System.Text.Json;

namespace CHDesktop.Services.Server;

class ReportHttp : MainHttp
{
    public async Task<LoggingHelper> SendDataAnalytic(List<int> idsComplaints)
    {
        string uri = $"https://{IP}:{Port}/report/doAnalytics";
        string jsonRequestBody = JsonSerializer.Serialize(idsComplaints);
        ResponseModel responseModel = await PostRequest(uri, jsonRequestBody);
        return responseModel.LoggingAnswer;
    }

    public async Task<(LoggingHelper, List<Report>?)> GetReports()
    {
        string uri = $"https://{IP}:{Port}/report/reports";
        ResponseModel responseModel = await GetRequest(uri);
        List<Report>? reports = !string.IsNullOrEmpty(responseModel.RequestObject) ? JsonSerializer.Deserialize<List<Report>>(responseModel.RequestObject) : null;
        return (responseModel.LoggingAnswer, reports);
    }

    public async Task<(LoggingHelper, ReportSummary?)> GetReportById(int reportId)
    {
        string uri = $"https://{IP}:{Port}/report/reportId/{reportId}";
        ResponseModel responseModel = await GetRequest(uri);
        ReportSummary? reports = !string.IsNullOrEmpty(responseModel.RequestObject) ? JsonSerializer.Deserialize<ReportSummary?>(responseModel.RequestObject) : null;
        return (responseModel.LoggingAnswer, reports);
    }

    public async Task<LoggingHelper> DeleteReport(int idReport)
    {
        string uri = $"https://{IP}:{Port}/report/deleteReport/{idReport}";
        ResponseModel responseModel = await DeleteRequest(uri);
        return responseModel.LoggingAnswer;
    }
}
