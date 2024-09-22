using StoreManagement.Application.Common;
using StoreManagement.Application.DTOs.Request;

namespace StoreManagement.Application.Interfaces.IServices
{
    public interface ITableService
    {
        Task<TableDTO> CreateAsync(TableDTO tableDTO);
        Task<TableDTO> UpdateAsync(int id, TableDTO tableDTO);
        Task<bool> DeleteAsync(int id);
        Task<TableDTO> GetByIdAsync(int id);
        Task<PaginationResult<List<TableDTO>>> GetAllByIdStore(int id, string currentPage = "1", string pageSize = "9", bool filter = false, bool status = false);
        Task<int> GetCountAsync(int id);
    }
}
