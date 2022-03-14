using Microsoft.Extensions.Logging;
using SuperPanel.Abstractions;
using SuperPanel.Abstractions.Actions;
using SuperPanel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperPanel.Engines.Services
{
    public class GdprService : IGdprService
    {
        private readonly IHttpClientActions _httpClient;
        private readonly ILogger<GdprService> _logger;
        public GdprService(
            IHttpClientActions httpClient,
            ILogger<GdprService> logger
            )
        {
            _httpClient = httpClient;
            _logger = logger;
        }
        public async Task<User> DeleteUser(string userId)
        {
            var url = $@"http://localhost:61696/v1/contacts/{userId}";
            User user = null;
            try
            {
                user = await _httpClient.PutContentAsType<User>($"{url}/gdpr");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Problems deleting user with Gdpr Api: {ex.StackTrace}");
            }

            return user;
        }

        public async Task<User> SelectUser(string userId)
        {
            var url = $@"http://localhost:61696/v1/contacts/{userId}";
            User user = null;
            try
            {
                user = await _httpClient.ContentAsType<User>(url);
            }
            catch (Exception ex)
            {
                _logger.LogError($"The user is not present in Gdpr Api: {ex.StackTrace}");
            }

            return user;
        }
    }
}
