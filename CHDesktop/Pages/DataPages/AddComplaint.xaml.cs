
using CHDesktop.Models.Enums;
using CHDesktop.Models.ReportProcessing;
using CHDesktop.Services.Helpers;
using CHDesktop.Models.Domain;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using CHDesktop.Services.Server;

namespace CHDesktop.Pages.DataPages;

public partial class AddComplaint : Page
{
    public ObservableCollection<string> SelectedImagePaths { get; set; } = [];

    public AddComplaint()
    {
        InitializeComponent();
        CategoryCB.ItemsSource = UserInputValidator.CategoriesComplaint;
        LocationCB.ItemsSource = UserInputValidator.LocationCafes;
        DataContext = this;
    }
    private void DeleteImageMenuItem_Click(object sender, RoutedEventArgs e)
    {
        if (ListBoxPhoto.SelectedItem != null)
        {
            SelectedImagePaths.Remove((string)ListBoxPhoto.SelectedItem);
        }
    }

    private void ChooseImages_Click(object sender, RoutedEventArgs e)
    {
        OpenFileDialog openFileDialog = new()
        {
            Filter = "Image files (*.jpg, *.jpeg, *.png)|*.jpg; *.jpeg; *.png|All files (*.*)|*.*",
            Multiselect = true
        };

        if (openFileDialog.ShowDialog() == true)
        {
            try
            {
                foreach (string filePath in openFileDialog.FileNames)
                {
                    SelectedImagePaths.Add(filePath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке файлов: " + ex.Message);
            }
        }
    }

    private void BackButton_Click(object sender, RoutedEventArgs e)
    {
        WindowHelper.RunToBackPage();
    }

    private async void SendComplaint_Click(object sender, RoutedEventArgs e)
    {
        if (CategoryCB.SelectedIndex == -1)
        {
            MessageBox.Show("Необходимо выбрать категорию");
            return;
        }
        if (LocationCB.SelectedIndex == -1)
        {
            MessageBox.Show("Необходимо выбрать локацию");
            return;
        }
        ComplaintSummary complaintSummary = new()
        {
            MainComplaint = new()
            {
                Сategory = (СategoryComplaint)CategoryCB.SelectedIndex,
                Description = ComplaintBTC.BannerTextBox.Text,
                Location = (LocationCafe)LocationCB.SelectedIndex,
                Status = StatusComplaint.Open,
                CreateDate = DateTime.UtcNow
            },
            ListAttachmets = SelectedImagePaths.Select(image => new Attachment()
            {
                ImageBytes = WindowHelper.ConvertImageSourceToBytes(new BitmapImage(new Uri(image, UriKind.RelativeOrAbsolute)))
            }).ToList()
        };
        WindowHelper.StartLoadingGIF(true);
        LoggingHelper lh = await MainHttp.UserRequest.AddComplaint(complaintSummary);
        WindowHelper.StartLoadingGIF(false);
        if (WindowHelper.CheckStatusCode(lh))
        {
            CategoryCB.SelectedIndex = -1;
            LocationCB.SelectedIndex = -1;
            ComplaintBTC.BannerTextBox.Text = "";
            SelectedImagePaths.Clear();
            MessageBox.Show("Рекламация добавленна");
        }
    }
}
