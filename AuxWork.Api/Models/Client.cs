using System.ComponentModel.DataAnnotations;

namespace AuxWork.Api.Models
{
    public class Client
    {
        public Guid Id { get; set; }
        [MaxLength(160)] public string Name { get; set; } = default!;
        [MaxLength(200)] public string? ContactEmail { get; set; }
        public string? Notes { get; set; }
        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    }
}
