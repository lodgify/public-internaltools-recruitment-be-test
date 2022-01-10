using System;

namespace SuperPanel.API.Model
{
    public class DeletionRequest : Entity
    {
        public int Status { get; set; }
        public long UserId { get; set; }
        public int Priority { get; set; }
        public DateTime ProcessedDate { get; set; }
    }
}
