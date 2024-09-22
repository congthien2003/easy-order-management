using StoreManagement.Application.Common;
using StoreManagement.Application.DTOs.Request;
using StoreManagement.Application.DTOs.Response;

namespace StoreManagement.Application.Interfaces.IServices
{
    public interface IOrderDetailService
    {
        Task<List<OrderDetailDTO>> CreateByListAsync(List<OrderDetailDTO> orderDetailDTO);
        Task<OrderDetailDTO> UpdateAsync(OrderDetailDTO orderDetailDTO);
        Task<bool> DeleteAsync(int idOrder, int idFood);
        Task<int> GetCountAsync(int idOrder);
        Task<PaginationResult<List<OrderDetaiResponse>>> GetAllByIdOrderAsync(int idOrder, string currentPage = "1", string pageSize = "10");
    }
}
