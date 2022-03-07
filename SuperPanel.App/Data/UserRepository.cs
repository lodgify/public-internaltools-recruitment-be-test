using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SuperPanel.App.Infrastructure;
using SuperPanel.App.Models;
using SuperPanel.App.Models.Base.Output;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace SuperPanel.App.Data
{
    public interface IUserRepository
    {
        ListQueryResult<User> QueryAll(int page);
    }

    public class UserRepository : IUserRepository
    {
        private List<User> _users;
        private readonly IConfiguration _configuration;

        public UserRepository(IOptions<DataOptions> dataOptions, IConfiguration configuration)
        {
            _configuration = configuration;
            // preload the set of users from file.
            var json = System.IO.File.ReadAllText(dataOptions.Value.JsonFilePath);
            _users = JsonSerializer.Deserialize<IEnumerable<User>>(json)
                .ToList();
        }

        public ListQueryResult<User> QueryAll(int page)
        {
            var take = Convert.ToInt32(_configuration["PageSize"] ?? "12");
            if(page < 1) page = 1;

            var result = new ListQueryResult<User>
            {
                CurrentPage = page,
                IsLastPage = !(page * take < _users.Count),
                TotalPage = (int)Math.Ceiling(_users.Count / (take * 1.0)),
                Entities = _users.OrderBy(x => x.Id).Skip((page - 1) * take).Take(take).ToList()
            };

            return result;
        }

    }
}
