using Microsoft.EntityFrameworkCore;
using TestAssignment.Entity.Data;
using TestAssignment.Repository.Interfaces;

namespace TestAssignment.Repository.Implementations;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    protected readonly TestAssignmentContext _context;
    protected readonly DbSet<T> _dbSet;

    public GenericRepository(TestAssignmentContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }
}
