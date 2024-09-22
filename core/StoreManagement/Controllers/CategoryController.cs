using StoreManagement.Application.Interfaces.IServices;
using StoreManagement.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StoreManagement.Application.Common;
using StoreManagement.Application.DTOs.Request;

namespace StoreManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        [HttpPost]
        [Route("create")]
        public async Task<ActionResult<Result>> CreateAsync(CategoryDTO categoryDTO)
        {
            var result = await _categoryService.CreateAsync(categoryDTO);
            return Ok(Result<CategoryDTO?>.Success(result, "Tạo mới thành công"));
        }
        [HttpPut("update/{id:int}")]
        public async Task<ActionResult> UpdateAsync(int id, CategoryDTO categoryDTO)
        {
            var result = await _categoryService.UpdateAsync(id, categoryDTO);
            return Ok(Result<CategoryDTO?>.Success(result, "Cập nhật thành công"));
        }
        [HttpDelete("delete/{id:int}")]
        public async Task<ActionResult<Result>> DeleteAsync(int id)
        {
            var result = await _categoryService.DeleteAsync(id);
            return Ok(Result<bool>.Success(result,"Đã xóa thành công"));
        }
        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<Result>> GetCategoryById(int id)
        {
            var result = await _categoryService.GetByIdAsync(id);
            return Ok(Result<CategoryDTO?>.Success(result, "Lấy thông tin thành công"));
        }
        [HttpGet("search")]
        public async Task<ActionResult> GetCategoryByName(int idStore, string name)
        {
            var result = await _categoryService.GetByNameAsync(idStore, name);
            return Ok(result);
        }

        [HttpGet("all")]
        public async Task<ActionResult> GetAllByIdStore(int idStore)
        {

            var list = await _categoryService.GetByIdStore(idStore);
            if (list == null)
            {
                return BadRequest(Result.Failure("Không tìm thấy người dùng"));
            }
            return Ok(Result<List<CategoryDTO>>.Success(list, "Lấy thông tin thành công"));
        }

        [HttpGet("store")]
        public async Task<ActionResult> GetAllCategoryByIdStore(int idStore, string currentPage = "1", string pageSize = "5", string searchTerm = "")
        {

            var list = await _categoryService.GetAllByIdStoreAsync(idStore, currentPage, pageSize, searchTerm);
            if (list == null)
            {
                return BadRequest(Result.Failure("Không tìm thấy người dùng"));
            }
            return Ok(Result<PaginationResult<List<CategoryDTO>>>.Success(list, "Lấy thông tin thành công"));
        }
    }
}
