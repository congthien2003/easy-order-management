using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StoreManagement.Application.Common;
using StoreManagement.Application.DTOs.Request;
using StoreManagement.Application.DTOs.Response;
using StoreManagement.Application.Interfaces.IServices;
using StoreManagement.Services;

namespace StoreManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        private readonly IInvoiceService _invoiceService;
        public InvoiceController(IInvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
        }

        [HttpPost("create")]
        public async Task<ActionResult<Result>> CreateAsync(InvoiceDTO invoiceDTO)
        {
            var result = await _invoiceService.CreateAsync(invoiceDTO);
            return Ok(Result<InvoiceDTO?>.Success(result,"Tạo thành công"));
        }

        [HttpDelete("delete/{id:int}")]
        public async Task<ActionResult<Result>> DeleteAsync(int id)
        {
            var result = await _invoiceService.DeleteAsync(id);
            return Ok(Result<bool>.Success(result,"Xóa thành công"));
        }

        [HttpPut("update/{id:int}")]
        public async Task<ActionResult<Result>> UpdateAsync(int id, InvoiceDTO invoiceDTO)
        {
            var result = await _invoiceService.UpdateAsync(id, invoiceDTO);
            return Ok(Result<InvoiceDTO?>.Success(result,"Cập nhật thành công"));
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Result>> GetByIdAsync(int id)
        {
            var result = await _invoiceService.GetByIdAsync(id);
            return Ok(Result<InvoiceResponse?>.Success(result,"Lấy thông tin thành công"));
        }

        [HttpGet("accept/{id:int}")]
        public async Task<ActionResult<Result>> AcceptAsync(int id)
        {
            var result = await _invoiceService.Accept(id);
            return Ok(Result.Success("Xác nhận đơn hàng đã thanh toán thành công"));
        }

        [HttpGet("all")]
        public async Task<ActionResult> GetAllByIdStoreAsync(int idStore, string currentPage = "1", string pageSize = "5", string sortColumn = "", bool asc = false, bool filter = false, bool status = false)
        {
            var list = await _invoiceService.GetAllByIdStoreAsync(idStore, currentPage, pageSize, sortColumn, asc, filter, status);
            return Ok(Result<PaginationResult<List<InvoiceResponse>>>.Success(list, "Lấy thông tin thành công"));
        }
    }
}
