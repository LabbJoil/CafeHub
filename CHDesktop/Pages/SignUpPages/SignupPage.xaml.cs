
using CHDesktop.Models.Domain;
using CHDesktop.PersonalElements;
using CHDesktop.Services.Helpers;
using CHDesktop.Services.Server;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CHDesktop.Pages.UserPages;

public partial class SignupPage : Page
{
    public SignupPage()
    {
        InitializeComponent();
    }

    private async void CreateAccount_Click(object sender, RoutedEventArgs e)
    {
        if (!UserInputValidator.ValidateEmail(EmailBTC.BannerTextBox.Text) || !UserInputValidator.ValidatePassword(PasswordBPC.OriginalTextProperty))
            return;
        if (PhoneBTC.BannerTextBox.Text.Length < 5)
        {
            MessageBox.Show("Номер телефона должен быть от 5 символов");
            return;
        }
        if (FirstNameBTC.BannerTextBox.Text.Length < 4)
        {
            MessageBox.Show("Имя должено быть от 4 символов");
            return;
        }
        WindowHelper.StartLoadingGIF(true);
        var userState = await MainHttp.UserRequest.SignUpUser(new User()
        {
            Email = EmailBTC.BannerTextBox.Text,
            Password = PasswordBPC.OriginalTextProperty,
            FirstName = FirstNameBTC.BannerTextBox.Text,
            LastName = LastNameBTC.BannerTextBox.Text,
            Phone = PhoneBTC.BannerTextBox.Text,
        });

        WindowHelper.StartLoadingGIF(false);
        if (WindowHelper.CheckStatusCode(userState.Item1))
        {
            WindowHelper.BlockAuthorizeInterface(false);
            if (userState.Item2 == null)
                WindowHelper.PageFrame.Navigate(new ProfilePage());
            else
                WindowHelper.PageFrame.Navigate(new ProfilePage(userState.Item2));
        }
    }

    private void PhoneBTC_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        if (sender is BannerTextboxControl btc)
            e.Handled = UserInputValidator.ValidatePhone(btc, e);
    }

    private void BackButton_Click (object sender, RoutedEventArgs e)
    {
        WindowHelper.RunToBackPage();
    }
}
