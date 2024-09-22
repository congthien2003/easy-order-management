using StoreManagement.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreManagement.Domain.IRepositories
{
    public interface IProductSellRepository<TProductSell> where TProductSell : ProductSell
    {
        Task<TProductSell> CreateAsync(TProductSell productSell);
        Task<TProductSell> UpdateAsync(int id, TProductSell productSell, bool incluDeleted = false);
        Task<TProductSell> GetByIdAsync(int id, bool incluDeleted = false);
        Task<TProductSell> DeleteAsync(int id, bool incluDeleted = false);
    }
}
