using AutoMapper;
using StoreManagement.Application.DTOs.Request;
using StoreManagement.Application.Interfaces.IServices;
using StoreManagement.Domain.IRepositories;
using StoreManagement.Domain.Models;

namespace StoreManagement.Services
{
    public class StoreService : IStoreService
    {
        private readonly IMapper _mapper;
        private readonly IStoreRepository<Store> _storeRepository;
        public StoreService(IMapper mapper, IStoreRepository<Store> storeRepository)
        {
            _mapper = mapper;
            _storeRepository = storeRepository;
        }

        public async Task<StoreDTO> CreateAsync(StoreDTO storeDTO)
        {
            var store = _mapper.Map<Store>(storeDTO);
            var storeCreated = await _storeRepository.CreateAsync(store);
            return _mapper.Map<StoreDTO>(storeCreated);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            await _storeRepository.DeleteAsync(id);
            return true;
        }

        public async Task<List<StoreDTO>> GetAllAsync(int currentPage = 1, int pageSize = 5, string searchTerm = "", string sortColumn = "", bool ascSort = true, bool incluDeleted = false)
        {
            var listStore = await _storeRepository.GetAllAsync(currentPage,pageSize,searchTerm,sortColumn,ascSort,incluDeleted);
            return _mapper.Map<List<StoreDTO>>(listStore);
        }

        public async Task<StoreDTO> GetByIdAsync(int id)
        {
            var store = await _storeRepository.GetByIdAsync(id);
            return _mapper.Map<StoreDTO>(store);
        }

        public async Task<StoreDTO> GetByIdUserAsync(int id)
        {
            var store = await _storeRepository.GetByIdUserAsync(id);
            return _mapper.Map<StoreDTO>(store);
        }

        public async Task<List<StoreDTO>> GetByNameAsync(string name)
        {
            var store = await _storeRepository.GetByNameAsync(name);
            return _mapper.Map<List<StoreDTO>>(store);
        }

        public async Task<int> GetCountList(string searchTerm = "", bool incluDeleted = false)
        {
            var count = await _storeRepository.GetCount(searchTerm, incluDeleted);
            return count;
        }

        public async Task<StoreDTO> UpdateAsync(int id,StoreDTO storeDTO)
        {
            var storeUpdate = _mapper.Map<Store>(storeDTO); 
            var update = await _storeRepository.UpdateAsync(id, storeUpdate);
            return _mapper.Map<StoreDTO>(update);
        }
    }
}
