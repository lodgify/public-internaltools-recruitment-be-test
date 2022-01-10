
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace SuperPanel.API.NotificationsHub
{
    public class NotificationHub : Hub
    {

        public override async Task OnConnectedAsync()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "user");
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception ex)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "user");
            await base.OnDisconnectedAsync(ex);
        }
    }
}
