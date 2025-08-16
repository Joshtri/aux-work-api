namespace AuxWork.Api.Models
{
    public class TimeLog
    {
        public Guid Id { get; set; }

        public Guid WorkItemId { get; set; }
        public WorkItem? WorkItem { get; set; }

        public Guid? UserId { get; set; }
        public User? User { get; set; }

        public DateTime StartedAtUtc { get; set; } = DateTime.UtcNow;
        public int DurationMinutes { get; set; }  // > 0
        public decimal? RatePerHour { get; set; } // kalau ingin hitung biaya
        public string? Notes { get; set; }
    }
}
