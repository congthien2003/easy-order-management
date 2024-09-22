using StoreManagement.Application.DTOs.Auth;
using StoreManagement.Application.Interfaces.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoreManagement.Application.Common;
using StoreManagement.Application.DTOs.Request;

namespace StoreManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IJwtManager _jwtManager;

        public AuthController(IAuthenticationService authenticationService, IJwtManager jwtManager)
        {
            _authenticationService = authenticationService;
            _jwtManager = jwtManager;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult> Register(RegisterDTO request)
        {
            var result = await _authenticationService.Register(request);
            return Ok(Result<AuthResult>.Success(result, "Register Success"));
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult> Login(LoginDTO request)
        {
            var result = await _authenticationService.Login(request);
            return Ok(Result<AuthResult>.Success(result, "Login success"));
        }

        [HttpPost("change-password")]
        [Authorize]
        public async Task<ActionResult> ChangePassword(ChangePasswordDTO request)
        {
            var result = await _authenticationService.ChangePassword(request);
            return Ok(Result<AuthResult>.Success(result, "Change password success"));
        }

        [HttpPost("restore-password")]
        public async Task<ActionResult> RestorePassword(RestorePasswordDTO request)
        {
            var result = await _authenticationService.RestorePassword(request);
            return Ok(Result<AuthResult>.Success(result, "Restore password success"));
        }

        [HttpPost("logout")]
        public ActionResult Logout()
        {
            return Ok();
        }

        [HttpPost("refresh-token")]
        public ActionResult RefreshToken(UserDTO userDTO)
        {
            var token =_jwtManager.CreateToken(userDTO);
            if (token == null)
            {
                return BadRequest();
            }
            
            return Ok(new
            {
                token
            });
        }


    }
}

