using StoreManagement.Domain.Models;

namespace StoreManagement.Domain.IRepositories
{
    public interface IOrderRepository<TOrder> where TOrder : Order
    {
        Task<TOrder> CreateAsync(TOrder order);
        Task<TOrder> UpdateAsync(int id, TOrder order, bool incluDeleted = false);
        Task<TOrder> DeleteAsync(int id, bool incluDeleted = false);
        Task<TOrder> GetByIdAsync(int id, bool incluDeleted = false);
        Task<int> GetCountAsync(int idStore, string searchTerm = "", bool incluDeleted = false);
        Task<List<TOrder>> GetAllByIdStoreAsync(int idStore, string sortCol = "", bool ascSort = true, bool incluDeleted = false);
    }
}
