using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WerkstattlagerAPI.Models
{
    public class Manufacturer
    {
        [Key][Required] public int Id { get; set; }
        [Required][StringLength(50)] public string? Description { get; set; }
        [JsonIgnore] public ICollection<Device>? Devices { get; set; }
    }
}
