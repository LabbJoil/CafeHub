
using CHDesktop.Models.Enums;
using CHDesktop.PersonalElements;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace CHDesktop.Services.Helpers;

internal static class UserInputValidator
{
    public static readonly string[] LocationCafes = [
        "Кузнечный переулок, 3",
        "Новорощинская улица, 4",
        "Улица Федора Абрамова, 16к1",
        "Константиновский проспект, 23",
        "Кирпичный переулок, 8"
    ];

    public static string[] StatusesComplaint = [
        "Открыта",
        "На рассмотрении",
        "Закрыта"
    ];

    public static readonly string[] CategoriesComplaint = [
        "Еда",
        "Обстановка",
        "Персонал"
    ];

    public static bool ValidatePassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password) || password.Length < 6 ||
        !Regex.IsMatch(password, @"^(?=.*[A-Z])(?=.*\d)[A-Za-z\d!@#$%^&*()]+$"))
        {
            MessageBox.Show("Пароль должен состоять из латинских букв, длина пароля не меньше 6 символов с хотя бы одной цифрой и заглавной буквой.");
            return false;
        }
        return true;
    }

    public static bool ValidateEmail(string email)
    {
        try
        {
            var mailAddress = new System.Net.Mail.MailAddress(email);
            return mailAddress.Address == email;
        }
        catch
        {
            MessageBox.Show("Email некорректный");
            return false;
        }
    }

    public static bool ValidatePhone(BannerTextboxControl btc, TextCompositionEventArgs e)
    {
        if (!char.IsDigit(e.Text, 0) && e.Text != "+")
            return true;
        if (e.Text == "+" && btc.BannerTextBox.Text.Length > 0)
            return true;
        return false;
    }
}
