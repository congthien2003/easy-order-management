using StoreManagement.Application.DTOs.Request;
using StoreManagement.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreManagement.Application.Interfaces.IServices
{
    public interface IProductSellService
    {
        Task<ProductSellDTO> CreateAsync(ProductSellDTO productSellDTO);
        Task<ProductSellDTO> UpdateAsync(int id, ProductSellDTO productSellDTO);
        Task<bool> DeleteAsync(int id);
        Task<ProductSellDTO> GetByIdAsync(int id);
    }
}
