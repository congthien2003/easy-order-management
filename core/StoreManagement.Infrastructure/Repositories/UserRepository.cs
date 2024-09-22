using AutoMapper;
using StoreManagement.Infrastructure.Data;
using StoreManagement.Domain.Models;
using Microsoft.EntityFrameworkCore;
using StoreManagement.Application.DTOs.Auth;
using StoreManagement.Domain.IRepositories;
using System.Linq.Expressions;

namespace StoreManagement.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository<User>
    {
        private readonly DataContext _dataContext;
        private readonly IMapper mapper;

        public UserRepository(DataContext dataContext,
                              IMapper mapper)
        {
            _dataContext = dataContext;
            this.mapper = mapper;
        }

        public async Task<User> Delete(int id)
        {
            var user = await _dataContext.Users.Where(x => x.Id == id).FirstOrDefaultAsync();
            user.IsDeleted = true;
            await _dataContext.SaveChangesAsync();
            return user;
        }

        public async Task<User> Edit(User user)
        {
            var exists = await _dataContext.Users.FirstOrDefaultAsync(x => x.Id == user.Id);
            if (exists == null)
            {
                return null;
            }
            else
            {
                exists.Email = user.Email;
                exists.Username = user.Username;
                exists.Phones = !string.IsNullOrEmpty(user.Phones) ? user.Phones : "";
                exists.Role = user.Role;
            }
            await _dataContext.SaveChangesAsync();
            return exists;
        }

        public async Task<User> GetById(int id, bool includeDeleted = false)
        {
            return await _dataContext.Users.FirstOrDefaultAsync(x => x.Id == id && x.IsDeleted == includeDeleted);
        }

        public async Task<User> GetByEmail(string email, bool includeDeleted = false)
        {
            var user = await _dataContext.Users.Where(x => x.Email == email).FirstOrDefaultAsync();
            return user;
        }

        public async Task<User> GetByLogin(string email, string password)
        {
            var user = await _dataContext.Users.Where(x => x.Email == email && x.Password == password).FirstOrDefaultAsync();
            return user;
        }

        public async Task<User> CreateUser(User user)
        {
            _dataContext.Users.Add(user);
            await _dataContext.SaveChangesAsync();
            return user;
        }

        public async Task<User> UpdatePassword(int id, string password, bool includeDeleted = false)
        {
            var user = await _dataContext.Users.FirstOrDefaultAsync(x => x.Id == id);
            user.Password = password;
            _dataContext.Update(user);
            await _dataContext.SaveChangesAsync();
            return user;
        }

        public Expression<Func<User, object>> GetSortColumnExpression(string sortColumn)
        {
            switch (sortColumn)
            {
                case "username":
                    return x => x.Username;
                case "phones":
                    return x => x.Phones;
                case "email":
                    return x => x.Email;
                case "role":
                    return x => x.Role;
                default:
                    return x => x.Id;
            }
        }

        public async Task<List<User>> GetAll(string searchTerm = "", string sortCol = "", bool ascSort = true, bool incluDeleted = false)
        {
            var users = _dataContext.Users.AsQueryable();
            if (!string.IsNullOrEmpty(searchTerm))
            {
                users = users.Where(t => t.Username.Contains(searchTerm));
            }
            if (!incluDeleted)
            {
                users = users.Where(t => t.IsDeleted == incluDeleted);
            }
            if (!string.IsNullOrEmpty(sortCol))
            {
                if (ascSort)
                {
                    users = users.OrderByDescending(GetSortColumnExpression(sortCol.ToLower()));
                }
                else
                {
                    users = users.OrderBy(GetSortColumnExpression(sortCol.ToLower()));

                }
            }
            var list = await users.ToListAsync();
            return list;
        }
    }
}
