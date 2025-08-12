using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authorization;

namespace ECommerceAdmin.Hubs
{
    [Authorize]
    public class NotificationHub : Hub
    {
        public async Task JoinStoreGroup(string storeId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"store_{storeId}");
        }

        public async Task LeaveStoreGroup(string storeId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"store_{storeId}");
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await base.OnDisconnectedAsync(exception);
        }
    }
}
