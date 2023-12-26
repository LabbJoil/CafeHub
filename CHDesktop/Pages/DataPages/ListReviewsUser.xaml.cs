
using CHDesktop.Models.Domain;
using CHDesktop.Models.ReportProcessing;
using CHDesktop.Pages.Data;
using CHDesktop.Pages.UserPages;
using CHDesktop.Services.Helpers;
using CHDesktop.Services.Server;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace CHDesktop.Pages.DataPages;

public partial class ListReviewsUser : Page
{
    private readonly ObservableCollection<ComplaintSummary> ReceivedComplaints = [];
    private int SelectedComplaintID { get; set; }

    public ListReviewsUser()
    {
        InitializeComponent();
        ReviewsDataGrid.ItemsSource = ReceivedComplaints;
    }

    private async void Page_Loaded(object sender, RoutedEventArgs e)
    {
        WindowHelper.StartLoadingGIF(true);
        (LoggingHelper lh, List<ComplaintSummary>? complaints) = await MainHttp.UserRequest.GetComplaints();
        WindowHelper.StartLoadingGIF(false);
        if (!WindowHelper.CheckStatusCode(lh) || complaints == null)
            return;
        foreach (var complaint in complaints)
            ReceivedComplaints.Add(complaint);
    }

    private async void DeleteComplaint_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.Tag is ComplaintSummary complaint)
        {
            WindowHelper.StartLoadingGIF(true);
            LoggingHelper deleteComplaintLH = await MainHttp.UserRequest.DeleteComplaint(complaint.MainComplaint!.Id);
            WindowHelper.StartLoadingGIF(false);
            if (!WindowHelper.CheckStatusCode(deleteComplaintLH))
                return;
            ReceivedComplaints.Remove(complaint);
            if(SelectedComplaintID == complaint.MainComplaint!.Id)
            {
                CategoryTB.Text = "";
                LocationTB.Text = "";
                DescriptionBTC.BannerTextBox.Text = "";
                RealStatusTB.Text = "";
                RefreshDateTB.Text = "";
            }
        }
    }

    private void ViewComplaint_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.Tag is ComplaintSummary complaintSummary)
        {
            SelectedComplaintID = complaintSummary.MainComplaint!.Id;
            CategoryTB.Text = UserInputValidator.CategoriesComplaint[(int)complaintSummary.MainComplaint!.Сategory];
            LocationTB.Text = UserInputValidator.LocationCafes[(int)complaintSummary.MainComplaint!.Location];
            DescriptionBTC.BannerTextBox.Text = complaintSummary.MainComplaint!.Description;
            RealStatusTB.Text = UserInputValidator.StatusesComplaint[(int)complaintSummary.MainComplaint.Status];
            RefreshDateTB.Text = complaintSummary.MainComplaint.CreateDate.ToString();
        }
    }

    private void BackPage_Click(object sender, RoutedEventArgs e)
    {
        WindowHelper.RunToBackPage();
    }
}
