using AutoMapper;
using StoreManagement.Application.Common;
using StoreManagement.Application.DTOs.Request;
using StoreManagement.Application.Interfaces.IServices;
using StoreManagement.Domain.IRepositories;
using StoreManagement.Domain.Models;

namespace StoreManagement.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IMapper _mapper;
        private readonly ICategoryRepository<Category> _categoryRepository;

        public CategoryService(IMapper mapper , ICategoryRepository<Category> categoryRepository)
        {
            _mapper = mapper;
            _categoryRepository = categoryRepository;
        }

        public async Task<CategoryDTO> CreateAsync(CategoryDTO categoryDTO)
        {
            var category = _mapper.Map<Category>(categoryDTO);
            var categoryCreated = await _categoryRepository.CreateAsync(category);
            return _mapper.Map<CategoryDTO>(categoryCreated);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            await _categoryRepository.DeleteAsync(id);
            return true;
        }

        public async Task<PaginationResult<List<CategoryDTO>>> GetAllByIdStoreAsync(int id, string currentPage = "1", string pageSize = "5", string searchTerm = "",  bool incluDeleted = false)
        {
            int _currentPage = int.Parse(currentPage);
            int _pageSize = int.Parse(pageSize);

            var list = await _categoryRepository.GetAllByIdStoreAsync(id);
            var count = list.Count();
            list = list.Skip(_currentPage * _pageSize - _pageSize).Take(_pageSize).ToList();
            

            var listCategory = _mapper.Map<List<CategoryDTO>>(list);
            return PaginationResult<List<CategoryDTO>>.Create(listCategory, _currentPage, _pageSize, count);
        }

        public async Task<CategoryDTO> GetByIdAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            return _mapper.Map<CategoryDTO>(category);
        }

        public async Task<List<CategoryDTO>> GetByIdStore(int id)
        {
            var listCategory = await _categoryRepository.GetByIdStoreAsync(id);
            return _mapper.Map<List<CategoryDTO>>(listCategory);
        }

        public async Task<List<CategoryDTO>> GetByNameAsync(int idStore, string name)
        {
            var listCategoires = await _categoryRepository.GetByNameAsync(idStore,name);
            return _mapper.Map<List<CategoryDTO>>(listCategoires);
        }

        public async Task<int> GetCountList(int idStore, string searchTerm = "", bool incluDeleted = false)
        {
            var count = await _categoryRepository.GetCountAsync(idStore,searchTerm, incluDeleted);
            return count;
        }

        public async Task<CategoryDTO> UpdateAsync(int id, CategoryDTO categoryDTO)
        {
            var categoryUpdate = _mapper.Map<Category>(categoryDTO);
            var update = await _categoryRepository.UpdateAsync(id,categoryUpdate);
            return _mapper.Map<CategoryDTO>(update);
        }
    }
}
