
namespace StoreManagement.Application.Interfaces.IServices
{
    public interface INotificationService
    {
        public Task SendNotificationToOwner(string message);
    }
}
