using Microsoft.Extensions.Options;
using SuperPanel.App.Infrastructure;
using SuperPanel.App.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using SuperPanel.App.Infrastructure;
using System;

namespace SuperPanel.App.Data
{
    public interface IUserRepository
    {
        IEnumerable<User> QueryAll();

        public User QueryUser(int id);

        public int Get_Number_Users();

        public void Remove_User(User user);

    } 

    public class UserRepository : IUserRepository
    {
        private List<User> _users;
        private IOptions<DataOptions> _dataOptions;
        public UserRepository(IOptions<DataOptions> dataOptions)
        {
            // preload the set of users from file.
            var json = System.IO.File.ReadAllText(dataOptions.Value.JsonFilePath);
            _users = JsonSerializer.Deserialize<IEnumerable<User>>(json)
                .ToList();
            _dataOptions = dataOptions;
        }

        public User QueryUser(int id)
        {
            return _users.Find(u => u.Id == id);
        }
        public int Get_Number_Users()
        {
            return _users.Count();
        }
        public IEnumerable<User> QueryAll()
        {
            return _users;
        }

        public void Remove_User(User user)
        {
            _users.RemoveAll(u => u.Id == user.Id);
            var data = JsonSerializer.Serialize(_users);
            //Remove user from Json file so it is updated in the Index View
            try
            {
                System.IO.File.WriteAllTextAsync(_dataOptions.Value.JsonFilePath, data);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
