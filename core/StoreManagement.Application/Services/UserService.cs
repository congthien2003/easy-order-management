using AutoMapper;
using Microsoft.Win32;
using StoreManagement.Application.Common;
using StoreManagement.Application.DTOs.Auth;
using StoreManagement.Application.DTOs.Request;
using StoreManagement.Application.Interfaces.IServices;
using StoreManagement.Domain.IRepositories;
using StoreManagement.Domain.Models;

namespace StoreManagement.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository<User> _userRepository;
        private readonly IMapper mapper;
        private readonly IJwtManager _jwtManager;

        public UserService(
                           IUserRepository<User> userRepository,
                           IMapper mapper,
                           IJwtManager jwtManager)
        {
            _userRepository = userRepository;
            this.mapper = mapper;
            this._jwtManager = jwtManager;
        }

        public async Task<bool> Delete(int id)
        {
            await _userRepository.Delete(id);
            return true;
        }

        public async Task<UserDTO> Edit(UserDTO userDTO)
        {
            var user = mapper.Map<User>(userDTO);
            var edit = await _userRepository.Edit(user);
            return mapper.Map<UserDTO>(edit);
        }

        public async Task<UserDTO> Login(LoginDTO loginDTO)
        {
            var login = await _userRepository.GetByLogin(loginDTO.Email, loginDTO.Password);
            return mapper.Map<UserDTO>(login);
        }

        public async Task<UserDTO?> GetById(int id, bool includeDeleted = false)
        {
            var user = await _userRepository.GetById(id, includeDeleted);
            return  mapper.Map<UserDTO>(user) ?? null;
        }

        public async Task<UserDTO> GetByEmail(string email, bool includeDeleted = false)
        {
            var user = await _userRepository.GetByEmail(email);
            return mapper.Map<UserDTO>(user);
        }

        public async Task<UserDTO> Register(RegisterDTO registerDTO)
        {
            registerDTO.Password = _jwtManager.getHashpassword(registerDTO.Password);
            var newUser = mapper.Map<User>(registerDTO);
           await _userRepository.CreateUser(newUser);
           return mapper.Map<UserDTO>(newUser);
        }

        public async Task<UserDTO> UpdatePassword(int id, string password, bool includeDeleted = false)
        {
            var user = await _userRepository.UpdatePassword(id, password, includeDeleted);
            return mapper.Map<UserDTO>(user);
        }

        public async Task<PaginationResult<List<UserDTO>>> GetAll(string currentPage = "1", string pageSize = "5", string searchTerm = "", string sortColumn = "", string asc = "true")
        {
            int _currentPage = int.Parse(currentPage);
            int _pageSize = int.Parse(pageSize);
            bool _asc = bool.Parse(asc);

            var list = await _userRepository.GetAll();
            var count = list.Count();

            list = list.Skip(_currentPage * _pageSize - _pageSize).Take(_pageSize).ToList();

            var listUser = mapper.Map<List<UserDTO>>(list);
            return PaginationResult<List<UserDTO>>.Create(listUser, _currentPage, _pageSize, count);
        }
    }
}
