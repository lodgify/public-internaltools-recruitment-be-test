using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SuperPanel.App.Models.DTO;
using SuperPanel.App.Models.DTO.Base;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SuperPanel.App.Service
{
    public class ApiService : IApiService
    {
        private readonly ILogger<ApiService> _logger;
        private readonly IConfiguration _configuration;
        private string _baseUri;
        public ApiService(ILogger<ApiService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _baseUri = _configuration["ExternalContactsBaseUri"];
        }
        public async Task<SingleQueryResult<GdprCommandResult>> PutGdpr(int id)
        {
            var res = new SingleQueryResult<GdprCommandResult>();

            try
            {
                var url = $"{_baseUri}/v1/contacts/{id}/gdpr";
                var httpContent = new StringContent("", Encoding.UTF8, "application/json");
                using var client = new HttpClient();
                using HttpResponseMessage responseMessage = await client.PutAsync(url, httpContent);

                var responseString = await responseMessage.Content.ReadAsStringAsync();

                if(responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var result = JsonConvert.DeserializeObject<GdprCommandResult>(responseString);
                    res.Entity = result;
                }
                else if(responseMessage.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    res.ErrorMessage = "Entity  not found!";
                }
                else
                {
                    res.ErrorMessage = "Unexpected error occured. Try again later.";
                }

                return res;
            }
            catch (Exception e)
            {
                _logger.LogError("Error occured while calling GDPR service!");
                res.ErrorMessage = e.Message;
            }

            return res;
        }
    }
}
