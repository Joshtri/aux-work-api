using Microsoft.EntityFrameworkCore;
using AuxWork.Api.Models; // <- pastikan namespace models kamu benar

namespace AuxWork.Api.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        // DbSets (sesuaikan dengan Models yang sudah kamu buat)
        public DbSet<User> Users => Set<User>();
        public DbSet<Client> Clients => Set<Client>();
        public DbSet<Project> Projects => Set<Project>();
        public DbSet<ProjectMember> ProjectMembers => Set<ProjectMember>();
        public DbSet<Label> Labels => Set<Label>();
        public DbSet<WorkItem> WorkItems => Set<WorkItem>();
        public DbSet<WorkItemLabel> WorkItemLabels => Set<WorkItemLabel>();
        public DbSet<ChecklistItem> ChecklistItems => Set<ChecklistItem>();
        public DbSet<Comment> Comments => Set<Comment>();
        public DbSet<TimeLog> TimeLogs => Set<TimeLog>();
        public DbSet<Attachment> Attachments => Set<Attachment>();

        protected override void OnModelCreating(ModelBuilder b)
        {
            base.OnModelCreating(b);

            // contoh relasi penting (opsional, tapi bagus ada)
            b.Entity<WorkItem>()
                .HasOne(w => w.Parent)
                .WithMany(p => p.Children)
                .HasForeignKey(w => w.ParentId)
                .OnDelete(DeleteBehavior.SetNull);

            b.Entity<WorkItemLabel>()
                .HasKey(x => new { x.WorkItemId, x.LabelId });

            b.Entity<ProjectMember>()
                .HasKey(x => new { x.ProjectId, x.UserId });
        }
    }
}
