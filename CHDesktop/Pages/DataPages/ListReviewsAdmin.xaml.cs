
using CHDesktop.Models.Enums;
using CHDesktop.Models.ReportProcessing;
using CHDesktop.Services.Helpers;
using CHDesktop.Services.Server;
using CHDesktop.Windows;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace CHDesktop.Pages.DataPages;

public partial class ListReviewsAdmin : Page
{
    private readonly ObservableCollection<ComplaintSummary> ReceivedComplaints = [];
    private int SelectedComplaintID { get; set; } = -1;

    public ListReviewsAdmin()
    {
        InitializeComponent();
        StatusCB.ItemsSource = UserInputValidator.StatusesComplaint;
        ReviewsDataGrid.ItemsSource = ReceivedComplaints;
    }

    private void CheckBox_Checked(object sender, RoutedEventArgs e)
    {
        if (sender is CheckBox checkBox)
            if (ReviewsDataGrid.SelectedItem is ComplaintSummary complaintSummary)
                complaintSummary.IsSelected = checkBox.IsChecked ?? false;
    }

    private async void ChangeStatus_Click(object sender, RoutedEventArgs e)
    {
        if (SelectedComplaintID == -1)
            return;
        StatusComplaint newStatus = (StatusComplaint)StatusCB.SelectedIndex;
        WindowHelper.StartLoadingGIF(true);
        LoggingHelper lh = await MainHttp.UserRequest.EditStatusComplaint(SelectedComplaintID, newStatus);
        WindowHelper.StartLoadingGIF(false);
        if (WindowHelper.CheckStatusCode(lh))
            MessageBox.Show("Статус изменен");
        else
            StatusCB.SelectedIndex = (int)ReceivedComplaints.FirstOrDefault(elem => elem!.MainComplaint!.Id == SelectedComplaintID)!.MainComplaint!.Status;
    }

    private async void DoAnalysis_Click(object sender, RoutedEventArgs e)
    {
        List<int> idsComplaints = ReceivedComplaints.Where(c => c.IsSelected).Select(compl => compl.MainComplaint!.Id).ToList();
        WindowHelper.StartLoadingGIF(true);
        LoggingHelper lh = await MainHttp.ReportRequest.SendDataAnalytic(idsComplaints);
        WindowHelper.StartLoadingGIF(false);
        if (WindowHelper.CheckStatusCode(lh))
            MessageBox.Show("Идет анализ рекламаций. Результаты можно будет посмотреть в списке отчетов.");
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
            if (SelectedComplaintID == complaint.MainComplaint!.Id)
            {
                CategoryTB.Text = "";
                LocationTB.Text = "";
                DescriptionBTC.BannerTextBox.Text = "";
                StatusCB.SelectedIndex = -1;
                SelectedComplaintID = -1;
                PhoneTB.Text = "";
                FirstLastNameTB.Text = "";
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
            StatusCB.SelectedIndex = (int)complaintSummary.MainComplaint.Status;
            PhoneTB.Text = complaintSummary.UserComplain!.Phone;
            FirstLastNameTB.Text = $"{complaintSummary.UserComplain!.LastName} {complaintSummary.UserComplain!.FirstName}";
            if (complaintSummary.ListAttachmets?.Count > 0 && WindowHelper.ConvertByteToImage(complaintSummary!.ListAttachmets[0]!.ImageBytes!) is BitmapImage bi)
            {
                ListPhotosRC.UpdateImageStatic(bi);
                ListPhotosRC.RoundButton.Content = $"+{complaintSummary.ListAttachmets?.Count}";
            }
        }
    }

    private void BackPage_Click(object sender, RoutedEventArgs e)
    {
        WindowHelper.RunToBackPage();
    }

    private void ListPhotosRC_Click(object sender, RoutedEventArgs e)
    {
        var listAtta = ReceivedComplaints.FirstOrDefault(elem => elem!.MainComplaint!.Id == SelectedComplaintID)!.ListAttachmets;
        if(listAtta != null && listAtta.Count > 0)
        {
            ViewImagesWindow viewImagesWindow = new(listAtta);
            viewImagesWindow.Show();
        }
        
    }
}
