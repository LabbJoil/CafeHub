using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CafeHub.Models.Domain;

[Table("Attachment")]
public class Attachment
{
    [Key, Required]
    public int Id { get; set; }
    [Required]
    public int IdComplaint { get; set; }
    [Required]
    public byte[]? ImageBytes { get; set; }
}
