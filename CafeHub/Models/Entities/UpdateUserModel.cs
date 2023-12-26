namespace CafeHub.Models.Entities;

public class UpdateUserModel
{
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public byte[]? Icon { get; set; }
}
