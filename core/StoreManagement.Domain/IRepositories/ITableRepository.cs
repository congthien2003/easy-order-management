using StoreManagement.Domain.Models;

namespace StoreManagement.Domain.IRepositories
{
    public interface ITableRepository<TTable> where TTable : Table
    {
        Task<TTable> CreateAsync(TTable table);
        Task<TTable> UpdateAsync(int id, TTable table, bool incluDeleted = false);
        Task<TTable> DeleteAsync(int id, bool incluDeleted = false);
        Task<TTable> GetByIdAsync(int id, bool incluDeleted = false);
        Task<List<TTable>> GetAllByIdStore(int id, bool incluDeleted = false);
        Task<int> GetCountAsync(int id, bool incluDeleted = false);
    }
}
