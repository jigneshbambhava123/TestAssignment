using TestAssignment.Entity.Models;

namespace TestAssignment.Repository.Interfaces;

public interface IUserRepository
{
    Task<User> GetUserByEmailAsync(string email);
}
