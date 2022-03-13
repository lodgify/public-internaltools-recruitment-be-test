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
            try
            {
                var url = $@"http://localhost:61696/v1/contacts/{userId}";
                User user = await _httpClient.ContentAsType<User>(url);
                
                if(user != null)
                {
                    user = await _httpClient.PutContentAsType<User>($"{url}/gdpr");
                }

                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Problems deleting user with Gdpr Api: {ex.StackTrace}");
                throw;
            }
        }

        public async Task<User> SelectUser(string userId)
        {
            try
            {
                var url = $@"http://localhost:61696/v1/contacts/{userId}";
                var user = new User();
                try
                {
                    user = await _httpClient.ContentAsType<User>(url);
                }
                catch (Exception)
                {
                    user = null;
                }                

                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Problems deleting user with Gdpr Api: {ex.StackTrace}");
                throw;
            }
        }
    }
}
