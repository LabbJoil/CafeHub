
using System.Security.Cryptography;

namespace CafeHub.Services.System;

internal class PasswordHelper
{
    private static readonly byte[] SaltPassword = [0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08];

    public static string HashPassword(string password)
    {
        using var pbkdf2 = new Rfc2898DeriveBytes(password, SaltPassword, 10000, HashAlgorithmName.SHA384);
        byte[] hash = pbkdf2.GetBytes(32);
        string hashPassword = Convert.ToBase64String(hash);
        return hashPassword;
    }
}
