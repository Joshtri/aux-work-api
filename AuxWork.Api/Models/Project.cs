using System.ComponentModel.DataAnnotations;

namespace AuxWork.Api.Models
{
    public class Project
    {
        public Guid Id { get; set; }
        public Guid? ClientId { get; set; }
        public Client? Client { get; set; }

        [MaxLength(40)] public string? Code { get; set; }   // unik opsional (e.g. AUX)
        [MaxLength(200)] public string Name { get; set; } = default!;
        public bool IsArchived { get; set; }

        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAtUtc { get; set; } = DateTime.UtcNow;

    }
}
