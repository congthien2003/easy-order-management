using StoreManagement.Application.Common;
using StoreManagement.Application.DTOs.Request;

namespace StoreManagement.Application.Interfaces.IServices
{
    public interface ICategoryService
    {
        Task<CategoryDTO> CreateAsync(CategoryDTO categoryDTO);
        Task<CategoryDTO> UpdateAsync(int id, CategoryDTO categoryDTO);
        Task<bool> DeleteAsync(int id);
        Task<CategoryDTO> GetByIdAsync(int id);
        Task<List<CategoryDTO>>GetByIdStore(int id);
        Task<List<CategoryDTO>> GetByNameAsync(int idStore, string name);
        Task<PaginationResult<List<CategoryDTO>>> GetAllByIdStoreAsync(int id, string currentPage = "1", string pageSize = "5", string searchTerm = "", bool incluDeleted = false);
        Task<int> GetCountList(int idStore,string searchTerm = "", bool incluDeleted = false);
    }
}
