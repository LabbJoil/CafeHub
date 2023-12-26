
using CHDesktop.Models.Enums;

namespace CHDesktop.Models.Domain;

public class Complaint
{
    public int Id { get; set; }
    public СategoryComplaint Сategory { get; set; }
    public string? Description { get; set; }
    public string? ShortDescription
    {
        get
        {
            if (Description?.Length > 20)
                return Description?[..20] + "...";
            else return Description;
        }
    }
    public StatusComplaint Status { get; set; }
    public LocationCafe Location { get; set; }
    public DateTime CreateDate { get; set; }
}
