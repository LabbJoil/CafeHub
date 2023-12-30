
using CafeHub.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CafeHub.Models.Domain;

[Table("Report")]
public class Report
{
    [Key, Required]
    public int Id { get; set; }
    [Required]
    public int IdUser { get; set; }
    [Required]
    public string? Name { get; set; }
    [Required]
    public int CountMessages { get; set; }
    [Required]
    public DateTime? CreateDate { get; set; }
}
