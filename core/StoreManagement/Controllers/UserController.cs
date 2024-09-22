using StoreManagement.Application.Interfaces.IServices;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StoreManagement.Application.Common;
using StoreManagement.Domain.Models;
using StoreManagement.Application.DTOs.Auth;
using Azure.Core;
using StoreManagement.Application.DTOs.Request;

namespace StoreManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
            
        }

        [HttpGet("getall")]
        public async Task<ActionResult<Result>> GetAll(string currentPage = "1", string pageSize = "5", string searchTerm = "", string sortColumn = "", string asc = "true")
        {
            var user = await _userService.GetAll(currentPage, pageSize, searchTerm, sortColumn, asc);
            if (user == null)
            {
                return BadRequest(Result.Failure("Không tìm thấy người dùng"));
            }
            return Ok(Result<PaginationResult<List<UserDTO>>>.Success(user, "Lấy thông tin thành công"));
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<Result>> GetUserByIdAsync(int id)
        {
            var user = await _userService.GetById(id);
            if (user == null)
            {
                return BadRequest(Result.Failure("Không tìm thấy người dùng"));
            }
            return Ok(Result<UserDTO?>.Success(user, "Lấy thông tin thành công"));
        }

        [HttpPut("update")]
        public async Task<ActionResult<Result>> Update(UserDTO user)
        {
            var update = await _userService.Edit(user);
            if (update == null)
            {
                return BadRequest(Result.Failure("Không tìm thấy người dùng"));
            }
            return Ok(Result<UserDTO?>.Success(update, "Cập nhật thông tin thành công"));
        }

        [HttpPost("create")]
        public async Task<ActionResult<Result>> Create(RegisterDTO user)
        {
            var update = await _userService.Register(user);
            if (update == null)
            {
                return BadRequest(Result.Failure("Đăng ký không thành công"));
            }
            return Ok(Result<UserDTO?>.Success(update, "Tạo mới người dùng thành công"));
        }

        [HttpDelete("delete")]
        public async Task<ActionResult<Result>> Delete(int id)
        {
            await _userService.Delete(id);
            return Ok(Result.Success("Xóa thành công"));
        }

    }
}
