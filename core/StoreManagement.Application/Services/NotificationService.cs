using Microsoft.AspNet.SignalR;
using StoreManagement.Application.Interfaces.IServices;
using StoreManagement.Application.RealTime;


namespace StoreManagement.Application.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IHubContext<OrderHub> _hubContext;

        public NotificationService(IHubContext<OrderHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task SendNotificationToOwner(string message)
        {
           
        }
    }
}
