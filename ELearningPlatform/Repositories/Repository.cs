using Microsoft.EntityFrameworkCore;
using ELearningPlatform.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ELearningPlatform.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    protected readonly ApplicationDbContext _db;
    protected readonly DbSet<T> _set;

    public Repository(ApplicationDbContext db)
    {
        _db  = db;
        _set = db.Set<T>();
    }

    public async Task<IEnumerable<T>> GetAllAsync()
        => await _set.AsNoTracking().ToListAsync();

    public async Task<T?> GetByIdAsync(int id)
        => await _set.FindAsync(id);

    public async Task<T> AddAsync(T entity)
    {
        await _set.AddAsync(entity);
        await _db.SaveChangesAsync();
        return entity;
    }

    public async Task UpdateAsync(T entity)
    {
        _set.Update(entity);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(T entity)
    {
        _set.Remove(entity);
        await _db.SaveChangesAsync();
    }
}