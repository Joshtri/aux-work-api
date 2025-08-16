namespace AuxWork.Api.Models
{
    public class ApiKey
    {
        public Guid Id { get; set; }
        public Guid? UserId { get; set; }
        public User? User { get; set; }

        public string KeyHash { get; set; } = default!; // simpan hash, bukan plaintext
        public string[] Scopes { get; set; } = Array.Empty<string>();

        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
        public DateTime? LastUsedAtUtc { get; set; }
    }
}
