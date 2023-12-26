using CHDesktop.Models.ReportProcessing;
using CHDesktop.Services.Helpers;
using CHDesktop.Services.Server;
using System.Windows;
using System.Windows.Controls;

namespace CHDesktop.Pages.UserPages;

public partial class ChangePasswordPage : Page
{
    public ChangePasswordPage()
    {
        InitializeComponent();
    }

    private async void UpdatePassword_Click(object sender, RoutedEventArgs e)
    {
        if (!UserInputValidator.ValidatePassword(NewPasswordBPC.OriginalTextProperty))
            return;
        UpdatePasswordModel updatePassword = new()
        {
            OldPassword = OldPasswordBPC.OriginalTextProperty,
            NewPassword = NewPasswordBPC.OriginalTextProperty
        };
        WindowHelper.BlockAuthorizeInterface(false);
        LoggingHelper userLH = await MainHttp.UserRequest.UpdatePassword(updatePassword);
        WindowHelper.BlockAuthorizeInterface(true);
        if (WindowHelper.CheckStatusCode(userLH))
        {
            OldPasswordBPC.ResertPassword();
            NewPasswordBPC.ResertPassword();
            MessageBox.Show("Вы успешно изменили пароль");
        }
    }

    private void BackPage_Click(object sender, RoutedEventArgs e)
    {
        WindowHelper.RunToBackPage();
    }
}
