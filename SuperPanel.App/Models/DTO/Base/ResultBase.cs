using System.Collections.Generic;

namespace SuperPanel.App.Models.DTO.Base
{
    public abstract class ResultBase
    {
        public bool Success => string.IsNullOrWhiteSpace(ErrorMessage) && (Errors == null || Errors.Count == 0);
        public string ErrorMessage { get; set; }
        public List<string> Errors { get; set; }
    }
}
