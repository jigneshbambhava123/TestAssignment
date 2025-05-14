using Microsoft.EntityFrameworkCore;
using TestAssignment.Entity.Data;
using TestAssignment.Entity.Models;
using TestAssignment.Repository.Interfaces;

namespace TestAssignment.Repository.Implementations;

public class UserRepository: GenericRepository<User>, IUserRepository
{
    public UserRepository(TestAssignmentContext context) : base(context)
    {
    }

    public async Task<User> GetUserByEmailAsync(string email)
    {
            return await _context.Users
                                    .Include(u => u.Role)
                                    .FirstOrDefaultAsync(u => u.Email == email);    
    }
}
