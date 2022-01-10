using System;

namespace SuperPanel.API.Dto
{
    public class DeletionRequestDto
    {
        public int Id { get; set; }
        public int Status { get; set; }
        public long UserId { get; set; }
        public int Priority { get; set; }
        public DateTime ProcessedDate { get; set; }
    }
}
