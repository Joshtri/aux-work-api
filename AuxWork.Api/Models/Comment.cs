using System.ComponentModel.DataAnnotations;

namespace AuxWork.Api.Models
{
    public class Comment
    {
        public Guid Id { get; set; }

        public Guid WorkItemId { get; set; }
        public WorkItem? WorkItem { get; set; }

        public Guid? AuthorId { get; set; }
        public User? Author { get; set; }

        [MaxLength(4000)] public string Body { get; set; } = default!;
        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    }
}
