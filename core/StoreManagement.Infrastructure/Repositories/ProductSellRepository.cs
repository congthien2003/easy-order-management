using Microsoft.EntityFrameworkCore;
using StoreManagement.Domain.IRepositories;
using StoreManagement.Domain.Models;
using StoreManagement.Infrastructure.Data;

namespace StoreManagement.Infrastructure.Repositories
{
    internal class ProductSellRepository : IProductSellRepository<ProductSell>
    {
        private readonly DataContext _dataContext;

        public ProductSellRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task<ProductSell> CreateAsync(ProductSell productSell)
        {
            var food = await _dataContext.Foods.FirstOrDefaultAsync(x => x.Id == productSell.FoodId && x.IsDeleted == false);
            if (food == null)
            {
                throw new NullReferenceException("Đồ ăn không tồn tại");
            }
            var newProductSell = await _dataContext.ProductSells.AddAsync(productSell);
            await _dataContext.SaveChangesAsync();
            return newProductSell.Entity;
        }

        public async Task<ProductSell> DeleteAsync(int id, bool incluDeleted = false)
        {
            var productSell = await _dataContext.ProductSells.FirstOrDefaultAsync(x => x.Id == id && x.IsDeleted == incluDeleted);
            if(productSell == null)
            {
                throw new NullReferenceException("Không tìm thấy sản phẩm");
            }
            _dataContext.ProductSells.Remove(productSell);
            await _dataContext.SaveChangesAsync();
            return productSell;
        }

        public async Task<ProductSell> GetByIdAsync(int id, bool incluDeleted = false)
        {
            var productSell = await _dataContext.ProductSells.FirstOrDefaultAsync(x => x.Id == id && x.IsDeleted == incluDeleted);
            if (productSell == null)
            {
                throw new NullReferenceException("Không tìm thấy sản phẩm");
            }
            return productSell;
        }

        public async Task<ProductSell> UpdateAsync(int id, ProductSell productSell, bool incluDeleted = false)
        {
            var update = await _dataContext.ProductSells.FirstOrDefaultAsync(x => x.Id == id && x.IsDeleted == incluDeleted);
            if (update == null)
            {
                throw new NullReferenceException("Không tìm thấy sản phẩm");
            }
            update.Quantity = productSell.Quantity;
            _dataContext.ProductSells.Update(update);
            await _dataContext.SaveChangesAsync();
            return update;
        }
    }
}
