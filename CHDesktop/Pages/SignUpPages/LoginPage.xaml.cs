
using CHDesktop.Models.Domain;
using CHDesktop.Services.Helpers;
using CHDesktop.Services.Server;
using System.Windows;
using System.Windows.Controls;

namespace CHDesktop.Pages.UserPages;

public partial class LoginPage : Page
{
    public LoginPage()
    {
        InitializeComponent();
    }

    private void BackButton_Click(object sender, RoutedEventArgs e)
    {
        WindowHelper.RunToBackPage();
    }

    private async void LogIn_Click(object sender, RoutedEventArgs e)
    {
        WindowHelper.StartLoadingGIF(true);
        (LoggingHelper lh, User? user) = await MainHttp.UserRequest.LogInUser(LogInBTC.BannerTextBox.Text, PasswordBPC.OriginalTextProperty);
        WindowHelper.StartLoadingGIF(false);
        if (WindowHelper.CheckStatusCode(lh))
        {
            WindowHelper.BlockAuthorizeInterface(false);
            if (user == null)
                WindowHelper.PageFrame.Navigate(new ProfilePage());
            else
                WindowHelper.PageFrame.Navigate(new ProfilePage(user));
        }
    }
}
