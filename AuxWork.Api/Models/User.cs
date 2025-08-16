using System.ComponentModel.DataAnnotations;

namespace AuxWork.Api.Models
{
    public class User
    {

        public Guid Id { get; set; }
        [MaxLength(120)] public string Name { get; set; } = default!;
        [MaxLength(200)] public string? Email { get; set; }
        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    }
}
