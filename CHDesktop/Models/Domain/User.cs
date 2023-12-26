
using CHDesktop.Models.Enums;

namespace CHDesktop.Models.Domain;

public class User
{
    public int Id { get; set; }
    public string? Password { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public UserRole Role { get; set; }
    public byte[]? Icon { get; set; }
}
