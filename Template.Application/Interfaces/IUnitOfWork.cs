using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Template.Application.Interfaces
{
    public interface IUnitOfWork
    {
        Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default);
        IRepository<TEntity> Repository<TEntity>() where TEntity : class;

        Task<int> GetNextSequence(Sequence sequence);
    }
}
