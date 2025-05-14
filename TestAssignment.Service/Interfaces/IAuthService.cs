using TestAssignment.Entity.Models;

namespace TestAssignment.Service.Interfaces;

public interface IAuthService
{
    Task<User> AuthenticateUserAsync(string email, string Password);

}
