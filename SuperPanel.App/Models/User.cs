using System;

namespace SuperPanel.App.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Login { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }

        public DateTime CreatedAt { get; set; }

        public User() { }

        public User(int id)
        {
            this.Id = id;
        }
    }

}
