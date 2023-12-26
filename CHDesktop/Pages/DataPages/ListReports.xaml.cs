
using CHDesktop.Models.Domain;
using CHDesktop.Pages.Data;
using CHDesktop.Services.Helpers;
using CHDesktop.Services.Server;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace CHDesktop.Pages.AuthorizePages;

public partial class ListReports : Page
{
    private readonly ObservableCollection<Report> ReceivedReports = [];

    public ListReports()
    {
        InitializeComponent();
        ReportsDataGrid.ItemsSource = ReceivedReports;
    }

    private async void ListReportsPage_Loaded(object sender, RoutedEventArgs e)
    {
        ReceivedReports.Clear();
        WindowHelper.StartLoadingGIF(true);
        var reports = await MainHttp.ReportRequest.GetReports();
        WindowHelper.StartLoadingGIF(false);
        if (!WindowHelper.CheckStatusCode(reports.Item1) || reports.Item2 == null)
            return;
        foreach (var report in reports.Item2)
            ReceivedReports.Add(report);
    }

    private async void DeleteReport_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.Tag is Report report)
        {
            WindowHelper.StartLoadingGIF(true);
            LoggingHelper deleteReportLH = await MainHttp.ReportRequest.DeleteReport(report.Id);
            WindowHelper.StartLoadingGIF(false);
            if (!WindowHelper.CheckStatusCode(deleteReportLH))
                return;
            ReceivedReports.Remove(report);
        }
    }

    private void ViewReport_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.Tag is Report report)
            WindowHelper.PageFrame.Navigate(new ResultAnalysis(report.Id));
    }

    private void BackButton_Click(object sender, RoutedEventArgs e)
    {
        WindowHelper.RunToBackPage();
    }
}
