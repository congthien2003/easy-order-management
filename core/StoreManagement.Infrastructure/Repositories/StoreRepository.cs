using AutoMapper;
using StoreManagement.Infrastructure.Data;
using StoreManagement.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using StoreManagement.Domain.IRepositories;

namespace StoreManagement.Infrastructure.Repositories
{
    public class StoreRepository : IStoreRepository<Store>
    {
        private readonly DataContext _dataContext;
        public StoreRepository(DataContext dataContext,
                              IMapper mapper)
        {
            _dataContext = dataContext;
        }
        public async Task<Store> CreateAsync(Store store)
        {
            var existingStore = await _dataContext.Stores.FirstOrDefaultAsync(s => s.Name == store.Name
                               && s.Address == store.Address);
            if (existingStore != null)
            {
                throw new InvalidOperationException("Cửa hàng đã tồn tại");
            }
            await _dataContext.Stores.AddAsync(store);
            await _dataContext.SaveChangesAsync();
            return store;
        }

        public async Task<Store> DeleteAsync(int id)
        {
            var store = await _dataContext.Stores.FirstOrDefaultAsync(x => x.Id == id  && x.IsDeleted == false);
            if (store == null)
            {
                throw new KeyNotFoundException("Không tìm thấy cửa hàng");
            }
            _dataContext.Stores.Remove(store);
            await _dataContext.SaveChangesAsync();
            return store;
        }

        public Task<List<Store>> GetAllAsync(int currentPage = 1, int pageSize = 5, string searchTerm = "", string sortCol = "", bool ascSort = true, bool incluDeleted = false)
        {
            var store = _dataContext.Stores.AsQueryable();
            if (!string.IsNullOrEmpty(searchTerm))
            {
                store = store.Where(t => t.Name.Contains(searchTerm));
            }
            if (!incluDeleted)
            {
                store = store.Where(t => t.IsDeleted == incluDeleted);
            }
            if (!string.IsNullOrEmpty(sortCol))
            {
                if (ascSort)
                {
                    store = store.OrderByDescending(GetSortColumnExpression(sortCol.ToLower()));
                }
                else
                {
                    store = store.OrderBy(GetSortColumnExpression(sortCol.ToLower()));

                }
            }
            var list = store.Skip(currentPage * pageSize - pageSize).Take(pageSize).ToListAsync();
            return list;
        }

        public async Task<Store> GetByIdAsync(int id, bool includeDeleted = false)
        {
            var store = await _dataContext.Stores.FirstOrDefaultAsync(x => x.Id == id && x.IsDeleted == includeDeleted);
            if(store == null)
            {
                throw new KeyNotFoundException("Cửa hàng không tồn tại");
            }
            return store;
        }

        public async Task<Store> GetByIdUserAsync(int id, bool includeDeleted = false)
        {
            var store = await _dataContext.Stores.FirstOrDefaultAsync(x => x.IdUser == id && x.IsDeleted == includeDeleted);
            if (store == null)
            {
                throw new KeyNotFoundException("Người dùng chưa có cửa hàng");
            }
            return store;
        }

        public async Task<List<Store>> GetByNameAsync(string name, bool includeDeleted = false)
        {
            var listStores = await _dataContext.Stores.Where(x=>x.Name.Contains(name)).ToListAsync();
            if(listStores.Count == 0)
            {
                throw new KeyNotFoundException("Cửa hàng không tồn tại");
            }
            return listStores;
        }
        public async Task<int> GetCount(string searchTerm = "", bool incluDeleted = false)
        {
            var store = _dataContext.Stores.AsQueryable();
            if (!string.IsNullOrEmpty(searchTerm))
            {
                store = store.Where(t => t.Name.Contains(searchTerm));
            }
            if (!incluDeleted)
            {
                store = store.Where(t => t.IsDeleted == incluDeleted);
            }
            var searchStore = await store.ToListAsync();
            return searchStore.Count();
        }
        public Expression<Func<Store, object>> GetSortColumnExpression(string sortColumn)
        {
            switch (sortColumn)
            {
                case "name":
                    return x => x.Name;
                case "address":
                    return x => x.Address;
                default:
                    return x => x.Id;
            }
        }
        public async Task<Store> UpdateAsync(int id, Store store, bool includeDeleted = false)
        {
            var storeUpdate = await _dataContext.Stores.FirstOrDefaultAsync(x => x.Id == id && x.IsDeleted == false);
            if (storeUpdate == null)
            {
                throw new KeyNotFoundException("Không tìm thấy cửa hàng");
            }
            storeUpdate.Name = store.Name;
            storeUpdate.Phone = store.Phone;
            storeUpdate.Address = store.Address;
            _dataContext.Stores.Update(storeUpdate);
            await _dataContext.SaveChangesAsync();
            return storeUpdate;
        }
    }
}
