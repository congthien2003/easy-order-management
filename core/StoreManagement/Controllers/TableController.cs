using StoreManagement.Application.Interfaces.IServices;
using Microsoft.AspNetCore.Mvc;
using StoreManagement.Application.Common;
using StoreManagement.Application.DTOs.Request;
namespace StoreManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TableController : ControllerBase
    {
        private readonly ITableService _tableService;
        private readonly IConfiguration _configuration;

        public TableController(ITableService tableService, IConfiguration configuration)
        {
            _configuration = configuration;
            _tableService = tableService;
        }
        [HttpPost("create")]
        public async Task<ActionResult<Result>> CreateAsync(TableDTO tableDTO)
        {
            var result = await _tableService.CreateAsync(tableDTO);
            return Ok(Result<TableDTO?>.Success(result,"Tạo mới thành công"));
        }
        [HttpPut("update/{id:int}")]
        public async Task<ActionResult<Result>> UpdateAsync(int id, TableDTO tableDTO)
        {

            var result = await _tableService.UpdateAsync(id, tableDTO);
            return Ok(Result<TableDTO?>.Success(result,"Cập nhật thành công"));
        }
        [HttpDelete("delete/{id:int}")]
        public async Task<ActionResult<Result>> DeleteAsync(int id)
        {

            var result = await _tableService.DeleteAsync(id);
            return Ok(Result<bool>.Success(result, "Xóa thành công"));
        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Result>> GetByIdAsync(int id)
        {

            var result = await _tableService.GetByIdAsync(id);
            return Ok(Result<TableDTO?>.Success(result, "Lấy thông tin thành công"));
        }
        [HttpGet("all")]
        public async Task<ActionResult> GetByIdStore(int idStore, string currentPage = "1", string pageSize = "9", bool filter = false, bool status = false)
        {
            var list = await _tableService.GetAllByIdStore(idStore, currentPage, pageSize, filter, status);
            if (list == null)
            {
                return BadRequest(Result.Failure("Không tìm thấy thông tin bàn"));
            }
            return Ok(Result<PaginationResult<List<TableDTO>>>.Success(list, "Lấy thông tin bàn thành công"));
        }

    }
}
