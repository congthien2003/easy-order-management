using AutoMapper;
using StoreManagement.Application.Common;
using StoreManagement.Application.DTOs.Request;
using StoreManagement.Application.Interfaces.IServices;
using StoreManagement.Domain.IRepositories;
using StoreManagement.Domain.Models;
using System.Collections.Generic;

namespace StoreManagement.Services
{
    public class FoodService : IFoodService
    {
        private readonly IMapper _mapper;
        private readonly IFoodRepository<Food> _foodRepository;

        public FoodService(IMapper mapper, IFoodRepository<Food> foodRepository) 
        {
            _mapper = mapper;
            _foodRepository = foodRepository;
        }
        public async Task<FoodDTO> CreateAsync(FoodDTO foodDTO)
        {
                var food = _mapper.Map<Food>(foodDTO);
                var foodCreated = await _foodRepository.CreateAsync(food);
                return _mapper.Map<FoodDTO>(foodCreated);       
        }

        public async Task<bool> DeleteAsync(int id)
        {
            await _foodRepository.DeleteAsync(id);
            return true;
        }

        public async Task<PaginationResult<List<FoodDTO>>> GetAllByIdStoreAsync(int id, string currentPage = "1", string pageSize = "5", string searchTerm = "", string sortColumn = "", bool asc = false, bool incluDeleted = false)
        {
            int _currentPage = int.Parse(currentPage);
            int _pageSize = int.Parse(pageSize);

            var list = await _foodRepository.GetAllByIdStoreAsync(id);
            var count = list.Count();
            list = list.Skip(_currentPage * _pageSize - _pageSize).Take(_pageSize).ToList();


            var listFood = _mapper.Map<List<FoodDTO>>(list);
            return PaginationResult<List<FoodDTO>>.Create(listFood, _currentPage, _pageSize, count);
        }

        public async Task<FoodDTO> GetByIdAsync(int id)
        {
            var food = await _foodRepository.GetByIdAsync(id);
            return _mapper.Map<FoodDTO>(food);
        }

        public async Task<PaginationResult<List<FoodDTO>>> GetByIdCategoryAsync(int id, string currentPage = "1", string pageSize = "5")
        {
            int _currentPage = int.Parse(currentPage);
            int _pageSize = int.Parse(pageSize);

            var list = await _foodRepository.GetByIdCategory(id);

            var count = list.Count();
            list = list.Skip(_currentPage * _pageSize - _pageSize).Take(_pageSize).ToList();
            var listFood = _mapper.Map<List<FoodDTO>>(list);
            return PaginationResult<List<FoodDTO>>.Create(listFood, _currentPage, _pageSize, count);

        }

        public async Task<List<FoodDTO>> GetByListId(int[] listId)
        {
            List<Food> listFood = new List<Food>();
            foreach (var id in listId)
            {
                listFood.Add(await _foodRepository.GetByIdAsync(id));
            }
            return _mapper.Map<List<FoodDTO>>(listFood);
        }

        public async Task<List<FoodDTO>> GetByNameAsync(int idStore, string name)
        {
            var listFood = await _foodRepository.GetByNameAsync(idStore, name);
            return _mapper.Map<List<FoodDTO>>(listFood);
        }

        public async Task<int> GetCountList(int idStore, string searchTerm = "", bool incluDeleted = false)
        {
            var count = await _foodRepository.GetCountAsync(idStore,  searchTerm, incluDeleted);
            return count;
        }

        public async Task<FoodDTO> UpdateAsync(int id, FoodDTO foodDTO)
        {
            var foodUpdate = _mapper.Map<Food>(foodDTO);
            var update = await _foodRepository.UpdateAsync(id, foodUpdate);
            return _mapper.Map<FoodDTO>(update);
        }
    }
}
