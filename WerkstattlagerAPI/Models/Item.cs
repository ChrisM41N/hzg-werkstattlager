using System.ComponentModel.DataAnnotations;

namespace WerkstattlagerAPI.Models
{
    public class Item
    {
        [Key][Required][StringLength(6)] public string? Id { get; set; }
        [Required][StringLength(50)] public string? SerialNumber { get; set; }
        [Required] public int DeviceId { get; set; }
        [Required] public Device? Device { get; set; }
        [Required] public DateTime DateIn { get; set; } = DateTime.UtcNow;
        public string? CommentIn { get; set; } = string.Empty;
        public DateTime? DateOut { get; set; }
        public string? CommentOut { get; set; }
        [Required] public bool IsInInventory { get; set; } = true;
    }
}
