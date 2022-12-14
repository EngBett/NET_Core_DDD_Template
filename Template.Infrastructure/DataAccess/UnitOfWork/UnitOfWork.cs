using Template.Application.Interfaces;
using Template.Infrastructure.Extensions;
using MediatR;
using System.Collections;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Template.Infrastructure.DataAccess.Repository;

namespace Template.Infrastructure.DataAccess.UnitOfWork
{
    public class UnitofWork : IUnitOfWork
    {

        private readonly ApplicationContext _context;
        private readonly IMediator _mediator;
        private Hashtable _repositories;
        public UnitofWork(ApplicationContext context, IMediator mediator)
        {
            _context = context;
            this._mediator = mediator;
        }
        public async Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var result = await _context.SaveChangesAsync(cancellationToken) > 0;
            await this._mediator.DispatchDomainEventsAsync(this._context);
            return result;
        }
        public void Dispose()
        {
            _context.Dispose();
        }

        public IRepository<TEntity> Repository<TEntity>() where TEntity : class
        {
            if (_repositories == null) _repositories = new Hashtable();
            var type = typeof(TEntity).Name;
            if (!_repositories.ContainsKey(type))
            {
                var repositoryType = typeof(Repository<>);
                var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), _context);
                _repositories.Add(type, repositoryInstance);
            }
            return (IRepository<TEntity>)_repositories[type];
        }

        public Task<int> GetNextSequence(Sequence sequence)
        {
            return this._context.GetNextSequence(sequence);
        }
    }
}
