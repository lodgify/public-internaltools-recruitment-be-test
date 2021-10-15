using ExternalContacts.Api.Infrastructure;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace ExternalContacts.Api.Services
{
    public interface IExternalContactsService
    {
        IEnumerable<Contact> GetAll();
        Contact GetByEmail(string email);
        Contact GetById(long id);

        void Anonymize(Contact c);
    }

    public class ExternalContactsService : IExternalContactsService
    {
        private readonly List<Contact> _contacts = new List<Contact>(1000);

        public ExternalContactsService(IOptions<DataOptions> dataOptions)
        {
            var json = System.IO.File.ReadAllText(dataOptions.Value.JsonFilePath);
            var users = JsonSerializer.Deserialize<IEnumerable<User>>(json)
                .OrderBy(_ => Guid.NewGuid())
                .Take(1000);
            
            var userIds = 10000;
            foreach (var user in users)
            {
                _contacts.Add(new Contact(userIds++)
                {
                    Email = user.Email.ToLowerInvariant(),
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    IsAnonymized = false
                });
            }

        }

        public IEnumerable<Contact> GetAll()
        {
            return _contacts;
        }

        public Contact GetByEmail(string email)
        {
            return _contacts.FirstOrDefault(_ => _.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        }

        public Contact GetById(long id)
        {
            return _contacts.FirstOrDefault(_ => _.Id == id);
        }

        public void Anonymize(Contact c)
        {
            var anonymizationTemplate = $"anonymous-{Guid.NewGuid()}";
            c.Email = $"{anonymizationTemplate}@externalservice.com";
            c.FirstName = anonymizationTemplate;
            c.LastName = anonymizationTemplate;
            c.IsAnonymized = true;
        }


        private class User
        {
            public int Id { get; set; }

            public string Login { get; set; }
            public string Email { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Phone { get; set; }

            public DateTime CreatedAt { get; set; }
        }
    }

    public class Contact
    {
        public long Id { get; private set; }

        public string Email { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public bool IsAnonymized { get; set; }


        public Contact(int id)
        {
            this.Id = id;
            this.IsAnonymized = false;
        }
    }

    
}
