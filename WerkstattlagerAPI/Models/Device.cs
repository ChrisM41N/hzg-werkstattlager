using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WerkstattlagerAPI.Models
{
    public class Device
    {
        [Key][Required] public int Id { get; set; }
        [Required][StringLength(50)] public string? Description { get; set; }
        [Required] public char CategoryId { get; set; }
        [Required] public Category? Category { get; set; }
        [Required] public int ManufacturerId { get; set; }
        [Required] public Manufacturer? Manufacturer { get; set; }
        [JsonIgnore] public ICollection<Item>? Items { get; set; }
    }
}
