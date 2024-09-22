using StoreManagement.Application.Interfaces.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using StoreManagement.Application.Common;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using StoreManagement.Application.DTOs.Request;

namespace StoreManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoreController : ControllerBase
    {
        private readonly IStoreService storeService;

        public StoreController(IStoreService storeService)
        {
            this.storeService = storeService;
        }
        [HttpPost]
        [Route("create")]
        public async Task<ActionResult> CreateAsync(StoreDTO storeDTO)
        {

            var result = await storeService.CreateAsync(storeDTO);
            return Ok(Result<StoreDTO?>.Success(result, "Tạo cửa hàng thành công"));
        }
        [HttpGet]
        [Route("all")]
        public async Task<ActionResult<Result>> GetAllStore(string currentPage = "1", string pageSize = "5", string searchTerm = "", string sortColumn = "", string asc = "true")
        {
            int _currentPage = int.Parse(currentPage);
            int _pageSize = int.Parse(pageSize);
            bool _asc = bool.Parse(asc);

            var list = await storeService.GetAllAsync(_currentPage, _pageSize, searchTerm, sortColumn, _asc);
            var count = await storeService.GetCountList(searchTerm);
            var _totalPage = count % _pageSize == 0 ? count / _pageSize : count / _pageSize + 1;
            var result = new
            {
                list,
                _currentPage,
                _pageSize,
                _totalPage,
                _totalRecords = count,
                _hasNext = _currentPage < _totalPage,
                _hasPre = _currentPage > 1,
            };
            return Ok(result);
        }
        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<Result>> GetStoreById(int id)
        {
            var result = await storeService.GetByIdAsync(id);
            return Ok(Result<StoreDTO?>.Success(result, "Lấy thông tin cửa hàng thành công"));
        }

        [HttpGet]
        [Route("idUser/{idUser:int}")]
        public async Task<ActionResult<Result>> GetStoreByIdUser(int idUser)
        {
            var result = await storeService.GetByIdUserAsync(idUser);
            return Ok(Result<StoreDTO?>.Success(result, "Lấy thông tin cửa hàng thành công"));
        }

        [HttpGet]
        [Route("search")]
        public async Task<ActionResult> GetStoreByName(string name)
        {
            var result = await storeService.GetByNameAsync(name);
            return Ok(result);
        }
        [HttpPut("update/{id:int}")]
        public async Task<ActionResult<Result>> UpdateStore(int id, StoreDTO storeDTO)
        {
            var result = await storeService.UpdateAsync(id, storeDTO);
            return Ok(Result<StoreDTO?>.Success(result, "Cập nhật thành công"));
        }
        [HttpDelete("delete/{id:int}")]
        public async Task<ActionResult<Result>> DeleteStore(int id)
        {
            var result = await storeService.DeleteAsync(id);
            return Ok(Result<bool>.Success(result, "Cập nhật thành công"));
        }
    }
}
