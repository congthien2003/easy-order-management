using StoreManagement.Application.Common;
using StoreManagement.Application.DTOs.Request;
using StoreManagement.Application.DTOs.Response;

namespace StoreManagement.Application.Interfaces.IServices
{
    public interface IOrderSerivce
    {
        Task<OrderDTO> CreateAsync(OrderDTO orderDTO);
        Task<OrderDTO> UpdateAsync(int id, OrderDTO orderDTO);
        Task<OrderDTO> AcceptOrder(int id);
        Task<bool> DeleteAsync(int id);
        Task<OrderResponse> GetByIdAsync(int id);
        Task<int> GetCountAsync(int idStore, string searchTerm = "");
        Task<PaginationResult<List<OrderResponse>>> GetAllByIdStoreAsync(int idStore, string currentPage = "1", string pageSize = "5", string sortCol = "", bool ascSort = true, bool filter = false, bool status = false);
    }
}
