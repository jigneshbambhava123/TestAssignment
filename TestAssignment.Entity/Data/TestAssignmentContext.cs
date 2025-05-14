using Microsoft.EntityFrameworkCore;
using TestAssignment.Entity.Models;

namespace TestAssignment.Entity.Data;

public class TestAssignmentContext : DbContext
{
    public TestAssignmentContext(DbContextOptions<TestAssignmentContext> options)
        : base(options)
    {
    }

    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<Role> Roles { get; set; }

}
