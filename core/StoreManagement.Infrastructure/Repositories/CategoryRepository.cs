using AutoMapper;
using StoreManagement.Infrastructure.Data;
using StoreManagement.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using StoreManagement.Domain.IRepositories;

namespace StoreManagement.Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository<Category>
    {
        private readonly DataContext _dataContext;

        public CategoryRepository(DataContext dataContext,
                                    IMapper mapper)
        {
            _dataContext = dataContext;
        }

        public async Task<Category> CreateAsync(Category category)
        {
            var store = await _dataContext.Stores.FirstOrDefaultAsync(x => x.Id == category.IdStore && x.IsDeleted == false);
            if (store == null)
            {
                throw new NullReferenceException("Không tồn tại cửa hàng");
            }
            var existCate = await _dataContext.Categories.FirstOrDefaultAsync(x => x.Name == category.Name && x.IdStore == category.IdStore);
            if (existCate != null)
            {
                throw new InvalidOperationException("Thể loại này đã tồn tại");
            }
            var newCategory = await _dataContext.Categories.AddAsync(category);
            await _dataContext.SaveChangesAsync();
            return newCategory.Entity;
        }

        public async Task<Category> DeleteAsync(int id)
        {
            var category = await _dataContext.Categories.FirstOrDefaultAsync(x => x.Id == id && x.IsDeleted == false);
            if(category == null)
            {
                throw new NullReferenceException("Không tìm thấy thể loại cần xóa");
            }
            _dataContext.Categories.Remove(category);
            await _dataContext.SaveChangesAsync();
            return category;
        }

        public Task<List<Category>> GetAllByIdStoreAsync(int id, string searchTerm = "", string sortCol = "", bool ascSort = true, bool incluDeleted = false)
        {
            var category = _dataContext.Categories.Where(x=>x.IdStore == id).AsQueryable();
            if (!string.IsNullOrEmpty(searchTerm))
            {
                category = category.Where(t => t.Name.Contains(searchTerm));
            }
            if (!incluDeleted)
            {
                category = category.Where(t => t.IsDeleted == incluDeleted);
            }
            if (!string.IsNullOrEmpty(sortCol))
            {
                if (ascSort)
                {
                    category = category.OrderByDescending(GetSortColumnExpression(sortCol.ToLower()));
                }
                else
                {
                    category = category.OrderBy(GetSortColumnExpression(sortCol.ToLower()));

                }
            }
            var list = category.ToListAsync();
            return list;
        }
        public Expression<Func<Category, object>> GetSortColumnExpression(string sortColumn)
        {
            switch (sortColumn)
            {
                case "name":
                    return x => x.Name;
                default:
                    return x => x.Id;
            }
        }
        public async Task<Category> GetByIdAsync(int id, bool incluDeleted = false)
        {
            var category = await _dataContext.Categories.FirstOrDefaultAsync(x => x.Id == id && x.IsDeleted == incluDeleted);
            if(category == null)
            {
                throw new KeyNotFoundException("Không tìm thấy thể loại");
            }
            return category;
        }

        public async Task<List<Category>> GetByNameAsync(int idStore, string name, bool incluDeleted = false)
        {
            var listCategories = await _dataContext.Categories.Where(x => x.Name.Contains(name) && x.IsDeleted == incluDeleted && x.Store.Id == idStore).ToListAsync();
            if(listCategories.Count == 0)
            {
                throw new KeyNotFoundException("Thể loại không tồn tại");
            }
            return listCategories;
        }

        public async Task<int> GetCountAsync(int idStore, string searchTerm = "", bool incluDeleted = false)
        {
            var category = _dataContext.Categories.Where(x => x.IdStore == idStore).AsQueryable();
            if (!string.IsNullOrEmpty(searchTerm))
            {
                category = category.Where(t => t.Name.Contains(searchTerm));
            }
            if (!incluDeleted)
            {
                category = category.Where(t => t.IsDeleted == incluDeleted);
            }
            var searchCategory = await category.ToListAsync();
            return searchCategory.Count();
        }

        public async Task<Category> UpdateAsync(int id, Category category, bool incluDeleted = false)
        {
            var categoryUpdate = await _dataContext.Categories.FirstOrDefaultAsync(x => x.Id == id && x.IsDeleted == incluDeleted);
            if(categoryUpdate == null)
            {
                throw new KeyNotFoundException("Không tìm thấy thể loại để cập nhật");
            }
            categoryUpdate.Name = category.Name;
            _dataContext.Categories.Update(categoryUpdate);
            await _dataContext.SaveChangesAsync();
            return categoryUpdate;
        }

        public async Task<List<Category>> GetByIdStoreAsync(int id, bool includeDeleted = false)
        {
            var store = await _dataContext.Stores.FirstOrDefaultAsync(x => x.Id == id && x.IsDeleted == includeDeleted);
            if (store == null)
            {
                throw new KeyNotFoundException("Cửa hàng không tồn tại");
            }
            var listCate = await _dataContext.Categories.Where(x=> x.Store.Id == id && x.IsDeleted== includeDeleted).ToListAsync(); 
            return listCate;
        }
    }
}
