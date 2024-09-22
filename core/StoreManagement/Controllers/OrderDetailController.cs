using StoreManagement.Application.Interfaces.IServices;
using StoreManagement.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StoreManagement.Application.Common;
using StoreManagement.Domain.Models;
using StoreManagement.Application.DTOs.Response;
using StoreManagement.Application.DTOs.Request;

namespace StoreManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderDetailController : ControllerBase
    {
        private readonly IOrderDetailService _orderDetailService;

        public OrderDetailController(IOrderDetailService orderDetailService)
        {
            _orderDetailService = orderDetailService;
        }
        [HttpPost("Create")]
        public async Task<ActionResult<Result>> CreateAsync(List<OrderDetailDTO> orderDetailDTO)
        {
            var result = await _orderDetailService.CreateByListAsync(orderDetailDTO);
            return Ok(Result<List<OrderDetailDTO>>.Success(result,"Tạo mới thành công"));
        }
        [HttpPut("update/{id:int}")]
        public async Task<ActionResult<Result>> UpdateAsync(OrderDetailDTO orderDetailDTO)
        {
            var result = await _orderDetailService.UpdateAsync(orderDetailDTO);
            return Ok(Result<OrderDetailDTO?>.Success(result, "Cập nhật thành công"));
        }
        [HttpDelete("delete/{id:int}")]
        public async Task<ActionResult<Result>> DeleteAsync(int idFood, int idOrder)
        {
            var result = await _orderDetailService.DeleteAsync(idFood, idOrder);
            return Ok(Result<bool>.Success(result, "Xóa thành công"));
        }
        [HttpGet("IdOrder/{idOrder:int}")]
        public async Task<ActionResult> GetAllByIdOrderAsync(int idOrder, string currentPage = "1", string pageSize = "10")
        {
            var list = await _orderDetailService.GetAllByIdOrderAsync(idOrder, currentPage, pageSize);
            if (list == null)
            {
                return BadRequest(Result.Failure("Không tìm thấy người dùng"));
            }
            return Ok(Result<PaginationResult<List<OrderDetaiResponse>>>.Success(list, "Lấy thông tin thành công"));
        }
    }
}
