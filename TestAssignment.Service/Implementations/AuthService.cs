using TestAssignment.Entity.Helpers;
using TestAssignment.Entity.Models;
using TestAssignment.Repository.Interfaces;
using TestAssignment.Service.Interfaces;

namespace TestAssignment.Service.Implementations;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;

    public AuthService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<User> AuthenticateUserAsync(string email, string password)
    {
        var user = await _userRepository.GetUserByEmailAsync(email);
        if(user != null  && PasswordHasher.VerifyPassword(password,user.PasswordHash)){
            
            return user;
        }
        return null;
    }
}
