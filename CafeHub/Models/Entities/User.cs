using CafeHub.Models.Entities;
using CafeHub.Models.Enums;
using CafeHub.Services.System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CafeHub.Models.Domain;

[Table("User")]
public class User
{
    [Key, Required]
    public int Id { get; set; }
    [Required]
    public string? Password { get; set; }
    [Required]
    public string? Email { get; set; }
    [Required]
    public string? Phone { get; set; }
    [Required]
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    [Required]
    public UserRole Role { get; set; }
    public byte[]? Icon { get; set; }

    public User() { }

    public User(User userModel)
    {
        if (!string.IsNullOrEmpty(userModel.Email)) Email = userModel.Email;
        if (!string.IsNullOrEmpty(userModel.FirstName)) FirstName = userModel.FirstName;
        if (!string.IsNullOrEmpty(userModel.Phone)) Phone = userModel.Phone;
        LastName = userModel.LastName;
        Icon = userModel.Icon;
        Role = UserRole.User;
        UpdatePassword(userModel.Password);
    }

    public void UpdateUser(UpdateUserModel userModel)
    {
        if (!string.IsNullOrEmpty(userModel.Email)) Email = userModel.Email;
        if (!string.IsNullOrEmpty(userModel.FirstName)) FirstName = userModel.FirstName;
        if (!string.IsNullOrEmpty(userModel.Phone)) Phone = userModel.Phone;
        LastName = userModel.LastName;
        Icon = userModel.Icon;
    }

    public void UpdatePassword(string? password)
    {
        if (!string.IsNullOrEmpty(password)) Password = PasswordHelper.HashPassword(password);
    }
}
