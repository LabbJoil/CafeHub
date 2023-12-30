
using CHDesktop.Models.Enums;

namespace CHDesktop.Models.Domain;

public class Report
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public int CountMessages { get; set; }
    public DateTime? CreateDate { get; set; }
}
