using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SuperPanel.Abstractions;
using SuperPanel.Config;
using SuperPanel.Engines.Derivates;
using SuperPanel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperPanel.Engines.Services
{
    public class DataService : IDataService
    {
        private readonly ILogger _logger;
        private readonly IOptions<DataOptions> _dataOptions;
        private readonly IUserRepository _userRepository;

        public DataService(
            ILogger<DataService> logger,
            IOptions<DataOptions> dataOptions,
            IUserRepository userRepository
            )
        {
            _logger = logger;
            _dataOptions = dataOptions;
            _userRepository = userRepository;
        }

        public List<User> GetPageListFromUsers(int? pageNumber)
        {
            try
            {
                var users = _userRepository.QueryAll();
                var pageList = PaginatedList<User>.CreatePageList(users.AsQueryable(), pageNumber ?? 1, _dataOptions.Value.PageSize);

                return pageList;
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Problems getting page list from users: {ex.StackTrace}");
                throw;
            }
        }
    }
}
