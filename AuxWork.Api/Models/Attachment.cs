using System.ComponentModel.DataAnnotations;

namespace AuxWork.Api.Models
{
    public class Attachment
    {
        public Guid Id { get; set; }
        public Guid WorkItemId { get; set; }
        public WorkItem? WorkItem { get; set; }

        [MaxLength(260)] public string FileName { get; set; } = default!;
        public string? ContentType { get; set; }
        public long? SizeBytes { get; set; }
        [MaxLength(1000)] public string StorageUrl { get; set; } = default!;

        public Guid? UploadedBy { get; set; }
        public User? Uploader { get; set; }
        public DateTime UploadedAtUtc { get; set; } = DateTime.UtcNow;
    }
}
