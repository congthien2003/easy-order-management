using StoreManagement.Domain.Models;

namespace StoreManagement.Domain.IRepositories
{
    public interface ICategoryRepository<TCategory> where TCategory : Category
    {
        Task<TCategory> CreateAsync(TCategory category);
        Task<TCategory> UpdateAsync(int id, TCategory category, bool inlcudeDeleted = false);
        Task<TCategory> DeleteAsync(int id);
        Task<TCategory> GetByIdAsync(int id, bool incluDeleted = false);
        Task<List<TCategory>> GetByIdStoreAsync(int id, bool incluDeleted = false);
        Task<int> GetCountAsync(int idStore, string searchTerm = "", bool incluDeleted = false);

        Task<List<TCategory>> GetAllByIdStoreAsync(int id, string searchTerm = "", string sortCol = "", bool ascSort = true, bool incluDeleted = false);
        Task<List<TCategory>> GetByNameAsync(int idStore, string name, bool incluDeleted = false);
    }
}
