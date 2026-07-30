using Microsoft.EntityFrameworkCore;
using ERP.Api.Data;
using ERP.Api.Interfaces;

namespace ERP.Api.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly ErpDbContext _context;
    private readonly DbSet<T> _dbSet;

    public Repository(ErpDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(new object[] { id });
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.AsNoTracking().ToListAsync();
    }

    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public void Update(T entity)
    {
        _dbSet.Update(entity);
    }

    public void Remove(T entity)
    {
        _dbSet.Remove(entity);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}
