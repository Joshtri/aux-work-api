using Microsoft.EntityFrameworkCore;

namespace AuxWork.Api.Models
{

    [PrimaryKey(nameof(ProjectId), nameof(UserId))]

    public class ProjectMember
    {
        public Guid ProjectId { get; set; }
        public Project? Project { get; set; }

        public Guid UserId { get; set; }
        public User? User { get; set; }

        public MemberRole Role { get; set; } = MemberRole.Collaborator;
        public DateTime AddedAtUtc { get; set; } = DateTime.UtcNow;
    }
}
