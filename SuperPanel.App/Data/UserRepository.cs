using Microsoft.Extensions.Options;
using SuperPanel.App.Infrastructure;
using SuperPanel.App.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace SuperPanel.App.Data
{
    public interface IUserRepository
    {
        IEnumerable<User> QueryAll();
    }

    public class UserRepository : IUserRepository
    {
        private List<User> _users;

        public UserRepository(IOptions<DataOptions> dataOptions)
        {
            // preload the set of users from file.
            var json = System.IO.File.ReadAllText(dataOptions.Value.JsonFilePath);
            _users = JsonSerializer.Deserialize<IEnumerable<User>>(json)
                .ToList();
        }

        public IEnumerable<User> QueryAll()
        {
            return _users;
        }

    }
}
