using AutoMapper;
using StoreManagement.Application.DTOs.Request;
using StoreManagement.Application.Interfaces.IServices;
using StoreManagement.Domain.IRepositories;
using StoreManagement.Domain.Models;

namespace StoreManagement.Application.Services
{
    public class ProductSellService : IProductSellService
    {
        private readonly IMapper _mapper;
        private readonly IProductSellRepository<ProductSell> _productSellRepository;

        public ProductSellService(IMapper mapper, IProductSellRepository<ProductSell> productSellRepository)
        {
            _mapper = mapper;
            _productSellRepository = productSellRepository;
        }
        public async Task<ProductSellDTO> CreateAsync(ProductSellDTO productSellDTO)
        {
            var productSell = _mapper.Map<ProductSell>(productSellDTO);
            var created = await _productSellRepository.CreateAsync(productSell);
            return _mapper.Map<ProductSellDTO>(created);    
        }

        public async Task<bool> DeleteAsync(int id)
        {
            await _productSellRepository.DeleteAsync(id);   
            return true;
        }

        public async Task<ProductSellDTO> GetByIdAsync(int id)
        {
            var productSell = await _productSellRepository.GetByIdAsync(id);
            return _mapper.Map<ProductSellDTO>(productSell);
        }

        public async Task<ProductSellDTO> UpdateAsync(int id, ProductSellDTO productSellDTO)
        {
            var update = _mapper.Map<ProductSell>(productSellDTO);
            var updated = await _productSellRepository.UpdateAsync(id, update);
            return _mapper.Map<ProductSellDTO>(updated);
        }
    }
}
