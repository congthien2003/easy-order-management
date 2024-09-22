
using Microsoft.AspNetCore.Mvc;
using StoreManagement.Application.Common;
using StoreManagement.Application.DTOs.Request;
using StoreManagement.Application.Interfaces.IServices;

namespace StoreManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentTypeController : ControllerBase
    {
        private readonly IPaymentTypeService _paymentTypeService;

        public PaymentTypeController(IPaymentTypeService paymentTypeService)
        {
            _paymentTypeService = paymentTypeService;
        }
        [HttpPost("create")]
        public async Task<ActionResult<Result>> CreateAsync(PaymentTypeDTO paymentTypeDTO)
        {
            var result = await _paymentTypeService.CreateAsync(paymentTypeDTO);
            return Ok(Result<PaymentTypeDTO?>.Success("Tạo mới thành công"));
        }
        [HttpPut("update/{id:int}")]
        public async Task<ActionResult<Result>> UpdateAsync(int id, PaymentTypeDTO paymentTypeDTO)
        {
            var result = await _paymentTypeService.UpdateAsync(id, paymentTypeDTO);
            return Ok(Result<PaymentTypeDTO?>.Success(result,"Cập nhật thành công"));
        }
        [HttpDelete("delete/{id:int}")]
        public async Task<ActionResult<Result>> DeleteAsync(int id)
        {
            var result = await _paymentTypeService.DeleteAsync(id);
            return Ok(Result<bool>.Success(result,"Xóa thành công"));
        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Result>> GetByIdAsync(int id)
        {
            var result = await _paymentTypeService.GetByIdAsync(id);
            return Ok(Result<PaymentTypeDTO?>.Success("Lấy thông tin thành công"));
        }
        [HttpGet("search")]
        public async Task<ActionResult<Result>> GetByNameAsync(int idStore, string name)
        {
            var results = await _paymentTypeService.GetByNameAsync(idStore, name);
            return Ok(results);
        }
        [HttpGet("all")]
        public async Task<ActionResult<Result>> GetAllByIdStoreAsync(int idStore, string currentPage = "1", string pageSize = "5", string searchTerm = "", string sortColumn = "", string asc = "true")
        {
            int _currentPage = int.Parse(currentPage);
            int _pageSize = int.Parse(pageSize);
            bool _asc = bool.Parse(asc);

            var list = await _paymentTypeService.GetAllByIdStoreAsync(idStore, _currentPage, _pageSize, searchTerm, sortColumn, _asc);
            var count = await _paymentTypeService.GetCountAsync(idStore, searchTerm);
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
    }
}
