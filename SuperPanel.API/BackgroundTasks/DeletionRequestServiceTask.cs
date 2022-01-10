using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SuperPanel.API.Dto;
using SuperPanel.API.Enums;
using SuperPanel.API.Model;
using SuperPanel.API.NotificationsHub;
using SuperPanel.API.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http.Headers;


namespace SuperPanel.API.BackgroundTasks
{
    public class DeletionRequestServiceTask : BackgroundService
    {
        private readonly SuperPanelContext _superPanelContext;
        private readonly IHubContext<NotificationHub> _hubContext;

        public DeletionRequestServiceTask(IServiceProvider serviceProvider, IHubContext<NotificationHub> hubContext)
        {
            _superPanelContext = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<SuperPanelContext>(); ;
            _hubContext = hubContext;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await CheckDeletionRequestPending();

                await Task.Delay(5000, stoppingToken);
            }
        }

        private async Task CheckDeletionRequestPending()
        {

            var pendingDeletionRequest = await GetDeletionRequestPending();

            if (pendingDeletionRequest != null && pendingDeletionRequest.Any())
            {
                foreach (var request in pendingDeletionRequest)
                {
                    // Call to API to delete (public ActionResult<Contact> Gdpr(long id))
                    // Route: {id:long}/gdpr
                    // http://localhost:61695/v1/contacts

                    var response = true; // Hence, we do not lower priority or request again

                    if (request.Status != (int)DeletionRequestEnum.State.Finalized) { 
                    response = DeleteUser(request.UserId, request);
                    }
                    // If fail -> modify status as Error and lower Priority 1 unit
                    if (!response)
                    {
                        request.Status = (int)DeletionRequestEnum.State.Error; 
                        request.Priority -= 1;
                    }
                    // If successful, modify Status as Finalized and Notify SignalR
                    else
                    {
                        request.Status = (int)DeletionRequestEnum.State.Finalized;
                    }
                    // Condition when deletion on API is Successful
                    if (response && request.Status == 4)
                    {
                        await _hubContext.Clients
                        .All
                        .SendAsync("DeletedUser", new { userId = request.UserId }); 
                        // Send the real userId 
                    }
                }
            }

        }
        private bool DeleteUser(long id, DeletionRequest request)
        {
            HttpClient client = new HttpClient();
            // /v1/contacts/{id}/gdpr (put)
            client.BaseAddress = new Uri("http://localhost:61695/v1/contacts/");
            client.DefaultRequestHeaders.Accept.Clear();

            // To Do: refactor to another method that serializes
            string userStr = $@"{{
              ""id"": {request.UserId},
              ""status"": {request.Status},
              ""userId"": {request.UserId},
              ""priority"": {request.Priority},
              ""processedDate"": {request.ProcessedDate}
            }}";
            var userJson = JsonConvert.SerializeObject(userStr);
            var buffer = System.Text.Encoding.UTF8.GetBytes(userJson);
            var user = new ByteArrayContent(buffer);
            user.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            //

            HttpResponseMessage response = client.PutAsync($"{id}/gdpr", user).Result;
            return response.IsSuccessStatusCode;
        }
        private async Task<IEnumerable<DeletionRequest>> GetDeletionRequestPending()
        {
            var deletionRequestPending = await _superPanelContext.DeletionRequests
                .Where(x => x.Status == (int)DeletionRequestEnum.State.Pending || 
                x.Status == (int)DeletionRequestEnum.State.Error)
                .OrderByDescending(x => x.Priority).ToListAsync();

            return deletionRequestPending;
        }
    }
}
