using ExpenseTracker.Core.Entities;

namespace ExpenseTracker.Core.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IRepository<T> Repository<T>() where T : BaseEntity;
    Task<int> CompleteAsync();
}
