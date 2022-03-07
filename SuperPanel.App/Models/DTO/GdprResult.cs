namespace SuperPanel.App.Models.DTO
{
    public class GdprCommandResult
    {
        public long Id { get; private set; }

        public string Email { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public bool IsAnonymized { get; set; }
    }
}
