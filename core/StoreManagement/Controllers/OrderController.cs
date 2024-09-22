
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using StoreManagement.Application.Common;
using StoreManagement.Application.DTOs.Request;
using StoreManagement.Application.DTOs.Response;
using StoreManagement.Application.Interfaces.IServices;
using StoreManagement.Application.RealTime;

namespace StoreManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderSerivce _OrderService;
        private readonly IHubContext<OrderHub> _hubContext;

        public OrderController(IOrderSerivce orderSerivce, IHubContext<OrderHub> hubContext)
        {
            _OrderService = orderSerivce;
            _hubContext = hubContext;
        }

        [HttpPost("create")]
        public async Task<ActionResult<Result>> CreateAsync(OrderDTO orderDTO)
        {   
            var result = await _OrderService.CreateAsync(orderDTO);
            if (result != null) {
                await _hubContext.Clients.Group(orderDTO.IdStore.ToString()).SendAsync("ReceiveNotification", "Có đơn đặt hàng vừa được tạo");
            }
            return Ok(Result<OrderDTO?>.Success(result,"Tạo mới thành công"));

        }

        [HttpDelete("delete/{id:int}")]
        public async Task<ActionResult<Result>> DeleteAsync(int id)
        {
            var result = await _OrderService.DeleteAsync(id);
            return Ok(Result<bool>.Success(result,"Xóa thành công"));
        }

        [HttpPut("update/{id:int}")]
        public async Task<ActionResult<Result>> UpdateAsync(int id, OrderDTO orderDTO)
        {
            var result = await _OrderService.UpdateAsync(id, orderDTO);
            return Ok(Result<OrderDTO?>.Success(result, "Cập nhật thành công"));
        }

        [HttpGet("accept/{id:int}")]
        public async Task<ActionResult<Result>> AcceptAsync(int id)
        {
            var result = await _OrderService.AcceptOrder(id);
            return Ok(Result<OrderDTO?>.Success(result, "Chấp nhận đơn đặt hàng thành công"));
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Result>> GetByIdAsync(int id)
        {
            var result = await _OrderService.GetByIdAsync(id);
            return Ok(Result<OrderResponse?>.Success(result, "Lấy thông tin thành công"));
        }
        [HttpGet("all")]
        public async Task<ActionResult> GetAllByIdStoreAsync(int idStore, string currentPage = "1", string pageSize = "5", string sortColumn = "", bool asc = false, bool filter = false, bool status = false)
        {
            var list = await _OrderService.GetAllByIdStoreAsync(idStore, currentPage, pageSize, sortColumn, asc, filter, status);
            return Ok(Result<PaginationResult<List<OrderResponse>>>.Success(list, "Lấy thông tin thành công"));
        }
    }
}
