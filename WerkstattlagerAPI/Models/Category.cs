using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WerkstattlagerAPI.Models
{
    public class Category
    {
        [Key][Required] public char Id { get; set; }
        [Required][StringLength(50)] public string? Description { get; set; }
        [JsonIgnore] public ICollection<Device>? Devices { get; set; }
    }
}
