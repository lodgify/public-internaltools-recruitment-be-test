using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SuperPanel.Abstractions;
using SuperPanel.Config;
using SuperPanel.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SuperPanel.DataProvider
{
    public class UserRepository : IUserRepository
    {
        private readonly ILogger _logger;
        private readonly DataOptions _dataOptions;

        public UserRepository(
            ILogger<UserRepository> logger,
            IOptions<DataOptions> dataOptions
            )
        {
            _logger = logger;
            _dataOptions = dataOptions.Value;
        }

        public List<User> QueryAll()
        {
            try
            {
                var users = GetUsersFromFile();

                return users;
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Problems getting users: {ex.StackTrace}");
                throw;
            }
        }

        private List<User> GetUsersFromFile()
        {
            var users = new List<User>();
            if (!string.IsNullOrEmpty(_dataOptions.JsonFilePath))
            {
                var json = File.ReadAllText(_dataOptions.JsonFilePath);
                users = JsonSerializer.Deserialize<List<User>>(json);
            }

            return users;
        }
    }
}
