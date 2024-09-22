using StoreManagement.Application.Common;
using StoreManagement.Application.DTOs.Request;

namespace StoreManagement.Application.Interfaces.IServices
{
    public interface IFoodService
    {
        Task<FoodDTO> CreateAsync(FoodDTO foodDTO);
        Task<FoodDTO> UpdateAsync(int id,FoodDTO foodDTO);
        Task<bool> DeleteAsync(int id);
        Task<FoodDTO> GetByIdAsync(int id);
        Task<List<FoodDTO>> GetByListId(int[] id);
        Task<List<FoodDTO>> GetByNameAsync(int idStore, string name);
        Task<PaginationResult<List<FoodDTO>>> GetByIdCategoryAsync(int id, string currentPage = "1", string pageSize = "5");
        Task<PaginationResult<List<FoodDTO>>> GetAllByIdStoreAsync(int id, string currentPage = "1", string pageSize = "5", string searchTerm = "", string sortColumn = "", bool asc = false, bool incluDeleted = false);
        Task<int> GetCountList(int idStore, string searchTerm = "", bool incluDeleted = false);
    }
}
