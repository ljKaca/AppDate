using API.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.SignalR
{
    [Authorize]
    public class PresenceHub : Hub
    {
        private readonly PresenceTracker presenceTracker;

        public PresenceHub(PresenceTracker presenceTracker)
        {
            this.presenceTracker = presenceTracker;
        }

        public override async Task OnConnectedAsync()
        {
            var isOnline = await presenceTracker.UserConnected(Context.User.GetUsername(), Context.ConnectionId);
            if(isOnline)
                      await Clients.Others.SendAsync("UserIsOnLine", Context.User.GetUsername());

            var currentUser = await presenceTracker.GetOnlineUsers();
            await Clients.Caller.SendAsync("GetOnLineUsers", currentUser);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var isOffline = await presenceTracker.UserDisconnected(Context.User.GetUsername(), Context.ConnectionId);
            if(isOffline)
                    await Clients.Others.SendAsync("UserIsOffLine", Context.User.GetUsername());
           
            await base.OnDisconnectedAsync(exception);
        }
    }
}
