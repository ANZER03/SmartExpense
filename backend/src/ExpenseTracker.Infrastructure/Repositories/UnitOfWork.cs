using System.Collections;
using ExpenseTracker.Core.Entities;
using ExpenseTracker.Core.Interfaces;
using ExpenseTracker.Infrastructure.Data;

namespace ExpenseTracker.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private readonly Hashtable _repositories;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
        _repositories = new Hashtable();
    }

    public IRepository<T> Repository<T>() where T : BaseEntity
    {
        var type = typeof(T).Name;

        if (!_repositories.ContainsKey(type))
        {
            var repositoryType = typeof(Repository<>);
            var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(T)), _context);
            _repositories.Add(type, repositoryInstance);
        }

        return (IRepository<T>)_repositories[type]!;
    }

    public async Task<int> CompleteAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
