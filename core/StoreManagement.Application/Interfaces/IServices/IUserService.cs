using StoreManagement.Application.Common;
using StoreManagement.Application.DTOs.Auth;
using StoreManagement.Application.DTOs.Request;
using StoreManagement.Domain.Models;

namespace StoreManagement.Application.Interfaces.IServices
{
    public interface IUserService
    {
        Task<UserDTO> Login(LoginDTO loginDTO);
        Task<UserDTO> Register(RegisterDTO registerDTO);
        Task<UserDTO?> GetById(int id, bool includeDeleted = false);
        Task<PaginationResult<List<UserDTO>>> GetAll(string currentPage = "1", string pageSize = "5", string searchTerm = "", string sortColumn = "", string asc = "true");

        Task<UserDTO> GetByEmail(string email, bool includeDeleted = false);
        Task<bool> Delete(int id);
        Task<UserDTO> Edit(UserDTO user);
        Task<UserDTO> UpdatePassword(int id, string password, bool includeDeleted = false);
    }
}
