using StoreManagement.Application.Interfaces.IServices;
using StoreManagement.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StoreManagement.Application.Common;
using System.Collections.Generic;
using StoreManagement.Application.DTOs.Request;

namespace StoreManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodController : ControllerBase
    {
        private readonly IFoodService _foodService;

        public FoodController(IFoodService foodService)
        {
            _foodService = foodService;
        }
        [HttpPost("Create")]
        public async Task<ActionResult<Result>> CreateAsync(FoodDTO foodDTO)
        {
            var result = await _foodService.CreateAsync(foodDTO);
            return Ok(Result<FoodDTO?>.Success(result, "Tạo mới thành công"));
        }
        [HttpPut("update/{id:int}")]
        public async Task<ActionResult<Result>> UpdateAsync(int id, FoodDTO foodDTO)
        {
            var result = await _foodService.UpdateAsync(id, foodDTO);
            return Ok(Result<FoodDTO?>.Success(result, "Cập nhật thành công"));
        }
        [HttpDelete("delete/{id:int}")]
        public async Task<ActionResult<Result>> DeleteAsync(int id)
        {
            var result = await _foodService.DeleteAsync(id);
            return Ok(Result<bool>.Success(result,"Xóa thành công"));
        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Result>> GetByIdAsync(int id)
        {

            var result = await _foodService.GetByIdAsync(id);
            return Ok(Result<FoodDTO?>.Success(result, "Lấy thông tin thành công"));
        }

        [HttpPost("listId")]
        public async Task<ActionResult<Result>> GetByListIdAsync([FromBody] int[] id)
        {
            var result = await _foodService.GetByListId(id);
            return Ok(Result<List<FoodDTO>?>.Success(result, "Lấy thông tin thành công"));
        }

        
        [HttpGet("search")]
        public async Task<ActionResult> GetByNameAsync(int idStore, string name)
        {
            var result = await _foodService.GetByNameAsync(idStore, name);
            return Ok(result);
        }
        [HttpGet("Store")]
        public async Task<ActionResult> GetAllFoodByIdStore(int idStore, string currentPage = "1", string pageSize = "5", string searchTerm = "")
        {

            var list = await _foodService.GetAllByIdStoreAsync(idStore, currentPage, pageSize, searchTerm);
            if (list == null)
            {
                return BadRequest(Result.Failure("Không tìm thấy người dùng"));
            }
            return Ok(Result<PaginationResult<List<FoodDTO>>>.Success(list, "Lấy thông tin thành công"));
        }
        [HttpGet("Category")]
        public async Task<ActionResult> GetFoodByIdCategory(int id, string currentPage = "1", string pageSize = "8")
        {
            var results = await _foodService.GetByIdCategoryAsync(id, currentPage, pageSize);
            return Ok(Result<PaginationResult<List<FoodDTO>>>.Success(results, "Lấy thông tin thành công"));
        }
    }
}
