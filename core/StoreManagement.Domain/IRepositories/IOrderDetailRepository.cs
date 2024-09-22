using StoreManagement.Domain.Models;

namespace StoreManagement.Domain.IRepositories
{
    public interface IOrderDetailRepository<TOrderDetail> where TOrderDetail : OrderDetail
    {
        Task<TOrderDetail> CreateAsync(OrderDetail orderDetail);
        Task<TOrderDetail> UpdateAsync(OrderDetail orderDetail);
        Task<TOrderDetail> DeleteAsync(int idOrder, int idFood);
        Task<List<TOrderDetail>> GetAllByIdOrderAsync(int idOrder);
        Task<int> GetCountAsync(int idOrder);
    }
}
