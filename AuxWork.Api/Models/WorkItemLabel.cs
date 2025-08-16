using Microsoft.EntityFrameworkCore;

namespace AuxWork.Api.Models
{
    [PrimaryKey(nameof(WorkItemId), nameof(LabelId))]
    public class WorkItemLabel
    {
        public Guid WorkItemId { get; set; }
        public WorkItem? WorkItem { get; set; }

        public Guid LabelId { get; set; }
        public Label? Label { get; set; }
    }
}
