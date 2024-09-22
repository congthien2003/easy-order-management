using StoreManagement.Application.DTOs.Request;

namespace StoreManagement.Application.Interfaces.IServices
{
    public interface IJwtManager
    {
        string CreateToken(UserDTO user);
        string getHashpassword(string password);
    }
}
