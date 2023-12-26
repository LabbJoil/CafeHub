
using CHDesktop.Models.Domain;
using CHDesktop.PersonalElements;
using CHDesktop.Services.Helpers;
using CHDesktop.Services.Server;
using CHDesktop.Models.Enums;
using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using CHDesktop.Pages.DataPages;
using CHDesktop.Pages.AuthorizePages;

namespace CHDesktop.Pages.UserPages;

public partial class ProfilePage : Page
{
    private User? NowUser { get; set; }

    public ProfilePage(User user)
    {
        InitializeComponent();
        //WindowHelper.PageFrame.Navigated += WindowHelper.RemoveAllBackPages;
        NowUser = user;
        FunctionalAdjustment();
        UpdateUser();
    }

    public ProfilePage()
    {
        Loaded += ProfilePage_Loaded;
    }

    private async void ProfilePage_Loaded(object sender, RoutedEventArgs e)
    {
        Loaded -= ProfilePage_Loaded;
        User? user = await LoadedUser();
        if (user == null)
            return;
        InitializeComponent();
        //WindowHelper.PageFrame.Navigated += WindowHelper.RemoveAllBackPages;
        if (user.Role == UserRole.Admin)
        {
            DeleteButton.IsEnabled = false;
            DeleteButton.Visibility = Visibility.Hidden;
        }
        NowUser = user;
        FunctionalAdjustment();
        UpdateUser();
    }

    private static async Task<User?> LoadedUser()
    {
        WindowHelper.StartLoadingGIF(true);
        (LoggingHelper LH, User? user) = await MainHttp.UserRequest.GetUserInfo();
        WindowHelper.StartLoadingGIF(false);
        WindowHelper.CheckStatusCode(LH);
        return user;
    }

    private void FunctionalAdjustment()
    {
        if (NowUser!.Role == UserRole.Admin)
        {
            AddReviewButton.IsEnabled = false;
            AddReviewButton.Visibility = Visibility.Hidden;
            DeleteButton.IsEnabled = false;
            DeleteButton.Visibility = Visibility.Hidden;
        }
        else
        {
            ListReportsButton.IsEnabled = false;
            ListReportsButton.Visibility = Visibility.Hidden;
        }
    }

    private void ChangePasswordB_Click(object sender, RoutedEventArgs e)
        => WindowHelper.PageFrame.Navigate(new ChangePasswordPage());

    private async void ChangeB_Click(object sender, RoutedEventArgs e)
    {
        if (!UserInputValidator.ValidateEmail(EMailBTC.BannerTextBox.Text))
            return;
        User updateUser = new()
        {
            Email = EMailBTC.BannerTextBox.Text,
            FirstName = FirstBTC.BannerTextBox.Text,
            LastName = LastNameBTC.BannerTextBox.Text,
            Phone = PhoneBTC.BannerTextBox.Text,
            Icon = WindowHelper.ConvertImageSourceToBytes(PersonalIconRC.ImageStaticButton)
        };
        WindowHelper.StartLoadingGIF(true);
        (LoggingHelper lh, User? user) = await MainHttp.UserRequest.UpdateUserInfo(updateUser);
        WindowHelper.StartLoadingGIF(false);
        if (WindowHelper.CheckStatusCode(lh))
        {
            MessageBox.Show("Аккаунт успешно обновлён");
            NowUser = user;
            UpdateUser();
        }
    }

    private async void DeleteUserB_Click(object sender, RoutedEventArgs e)
    {
        WindowHelper.StartLoadingGIF(true);
        LoggingHelper userLH = await MainHttp.UserRequest.DeleteUser();
        WindowHelper.StartLoadingGIF(false);
        if (WindowHelper.CheckStatusCode(userLH))
        {
            MessageBox.Show("Аккаунт успешно удалён");
            WindowHelper.PageFrame.Navigate(new AuthorizationPage());
        }
    }

    private void PhoneBTC_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        if (sender is BannerTextboxControl btc)
            e.Handled = UserInputValidator.ValidatePhone(btc, e);
    }

    private void AddReviewB_Click(object sender, RoutedEventArgs e)
    {
        WindowHelper.PageFrame.Navigate(new AddComplaint());
    }

    private void ListReportsB_Click(object sender, RoutedEventArgs e)
    {
        WindowHelper.PageFrame.Navigate(new ListReports());
    }

    private void ListReviewsB_Click(object sender, RoutedEventArgs e)
    {
        if (NowUser != null)
        {
            if (NowUser.Role == UserRole.Admin)
                WindowHelper.PageFrame.Navigate(new ListReviewsAdmin());
            else
                WindowHelper.PageFrame.Navigate(new ListReviewsUser());
        }
    }

    //------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    private void UpdateUser()
    {
        if (NowUser == null)
        {
            MessageBox.Show("Ошибка получения пользователя");
            return;
        }
        EMailBTC.BannerTextBox.Text = NowUser.Email;
        FirstBTC.BannerTextBox.Text = NowUser.FirstName;
        LastNameBTC.BannerTextBox.Text = NowUser.LastName;
        PhoneBTC.BannerTextBox.Text = NowUser.Phone;
        PersonalIconRC.ImageStaticButton = WindowHelper.ConvertByteToImage(NowUser.Icon);
    }

    private void ChooseNewProfileIcon_Click(object sender, RoutedEventArgs e)
    {
        OpenFileDialog openFileDialog = new()
        {
            Filter = "Image files (*.jpg, *.jpeg, *.png)|*.jpg; *.jpeg; *.png|All files (*.*)|*.*"
        };

        if (openFileDialog.ShowDialog() == true)
        {
            try
            {
                PersonalIconRC.UpdateImageStatic(new BitmapImage(new Uri(openFileDialog.FileName, UriKind.RelativeOrAbsolute)));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке файлов: " + ex.Message);
            }
        }
    }
}
