using CafeHub.Models.Enums;
using Microsoft.AspNetCore.Http.HttpResults;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CafeHub.Models.Domain;

[Table("Complaint")]
public class Complaint
{
    [Key, Required]
    public int Id { get; set; }
    [Required]
    public int IdUser { get; set; }
    public СategoryComplaint Сategory { get; set; }
    [Required]
    public string? Description { get; set; }
    public StatusComplaint Status { get; set; }
    public LocationCafe Location { get; set; }
    public DateTime CreateDate { get; set; }
}
