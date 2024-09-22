
using StoreManagement.Infrastructure.Data;
using StoreManagement.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using StoreManagement.Domain.IRepositories;

namespace StoreManagement.Infrastructure.Repositories
{
    public class FoodRepository : IFoodRepository<Food>
    {
        private readonly DataContext _dataContext;

        public FoodRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task<Food> CreateAsync(Food food)
        {
            var category = await _dataContext.Categories.FirstOrDefaultAsync(x => x.Id == food.IdCategory && x.IsDeleted == false);
            if (category == null)
            {
                throw new NullReferenceException("Không tồn tại thể loại này");
            }
            var exitsFood = await _dataContext.Foods.FirstOrDefaultAsync(x => x.Name == food.Name && x.Category.Id == food.IdCategory && x.IsDeleted == false);
            if (exitsFood != null)
            {
                throw new InvalidOperationException("Đồ ăn này đã tồn tại");
            }
            var newFood = await _dataContext.Foods.AddAsync(food);
            await _dataContext.SaveChangesAsync();
            return newFood.Entity;
        }

        public async Task<Food> DeleteAsync(int id)
        {
            var food = await _dataContext.Foods.FirstOrDefaultAsync(x => x.Id == id && x.IsDeleted == false);
            if (food == null)
            {
                throw new KeyNotFoundException("Không tìm thấy đồ ăn");
            }
            _dataContext.Foods.Remove(food);
            await _dataContext.SaveChangesAsync();
            return food;
        }
        public Expression<Func<Food, object>> GetSortColumnExpression(string sortColumn)
        {
            switch (sortColumn)
            {
                case "name":
                    return x => x.Name;
                default:
                    return x => x.Id;
            }
        }
        public Task<List<Food>> GetAllByIdStoreAsync(int id, string searchTerm = "", string sortCol = "", bool ascSort = true, bool incluDeleted = false)
        {
            var food = _dataContext.Foods.Where(x => x.Category.IdStore == id).AsQueryable();
            if (!string.IsNullOrEmpty(searchTerm))
            {
                food = food.Where(t => t.Name.Contains(searchTerm));
            }
            if (!incluDeleted)
            {
                food = food.Where(t => t.IsDeleted == incluDeleted);
            }
            if (!string.IsNullOrEmpty(sortCol))
            {
                if (ascSort)
                {
                    food = food.OrderByDescending(GetSortColumnExpression(sortCol.ToLower()));
                }
                else
                {
                    food = food.OrderBy(GetSortColumnExpression(sortCol.ToLower()));

                }
            }
            var list = food.ToListAsync();
            return list;
        }

        public async Task<Food> GetByIdAsync(int id, bool incluDeleted = false)
        {
            var food = await _dataContext.Foods.FirstOrDefaultAsync(x => x.Id == id && x.IsDeleted == incluDeleted);
            if (food == null)
            {
                throw new KeyNotFoundException("Không tìm thấy đồ ăn");
            }
            return food;
        }

        public async Task<List<Food>> GetByIdCategory(int id, bool incluDeleted = false)
        {
            var category = await _dataContext.Categories.FirstOrDefaultAsync(x => x.Id == id && x.IsDeleted == incluDeleted);
            if (category == null)
            {
                throw new KeyNotFoundException("Thể loại đồ ăn này không tồn tại");
            }
            var listFood = await _dataContext.Foods.Where(x => x.Category.Id == category.Id && x.IsDeleted == incluDeleted).ToListAsync();
            return listFood;
        }

        public async Task<List<Food>> GetByNameAsync(int idStore, string name, bool incluDeleted = false)
        {
            var listFoods = await _dataContext.Foods.Where(x => x.Name.Contains(name) && x.IsDeleted == incluDeleted && x.Category.IdStore == idStore).ToListAsync();
            if (listFoods.Count == 0)
            {
                throw new KeyNotFoundException("Không tìm thấy đồ ăn");
            }
            return listFoods;
        }

        public async Task<int> GetCountAsync(int idStore, string searchTerm = "", bool incluDeleted = false)
        {
            var food = _dataContext.Foods.Where(x => x.Category.IdStore == idStore).AsQueryable();
            if (!string.IsNullOrEmpty(searchTerm))
            {
                food = food.Where(t => t.Name.Contains(searchTerm));
            }
            if (!incluDeleted)
            {
                food = food.Where(t => t.IsDeleted == incluDeleted);
            }
            var searchFood = await food.ToListAsync();
            return searchFood.Count();
        }

        public async Task<Food> UpdateAsync(int id, Food food, bool incluDeleted = false)
        {
            var foodUpdate = await _dataContext.Foods.FirstOrDefaultAsync(x => x.Id == id && x.IsDeleted == incluDeleted);
            if (foodUpdate == null)
            {
                throw new KeyNotFoundException("Không tìm thấy đồ ăn cần chỉnh sửa");
            }
            foodUpdate.Name = food.Name;
            foodUpdate.Status = food.Status;
            _dataContext.Foods.Update(foodUpdate);
            await _dataContext.SaveChangesAsync();
            return foodUpdate;
        }
    }
}