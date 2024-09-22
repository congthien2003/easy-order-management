
using Microsoft.AspNetCore.SignalR;
namespace StoreManagement.Application.RealTime
{
    public class OrderHub : Hub
    {
        // Notify the owner of a new order
        public async Task NotifyOwner(string orderId, string storeId)
        {
            await Clients.Group(storeId).SendAsync("ReceiveOrderNotification", orderId);
        }

        // The owner joins the group for their store
        public async Task JoinStoreGroup(string storeId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, storeId);
        }

        // The owner leaves the group when they log out or disconnect
        public async Task LeaveStoreGroup(string storeId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, storeId);
        }
    }
}
