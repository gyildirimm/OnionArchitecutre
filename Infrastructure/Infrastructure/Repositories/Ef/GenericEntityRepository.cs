using Domain.Shared.Interfaces.Base;
using Domain.Shared.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Domain.Shared.Aggregates;
using System.Threading.Tasks;
using Application.Core.Wrappers;

namespace Infrastructure.Repositories.Ef
{
    public class GenericEntityRepository<TEntity, TContext> : IEntityRepository<TEntity>
       where TEntity : class, IEntity, new()
        where TContext : DbContext, IDbContext
    {
        private readonly DbContext _context;
        private DbSet<TEntity> _entity;

        public GenericEntityRepository(TContext context)
        {
            _context = context;
            _entity = _context.Set<TEntity>();
        }

        public IResponse<bool> Delete(TEntity entity)
        {
            bool result = false;

            var deletedEntity = _context.Entry(entity);
            deletedEntity.State = EntityState.Deleted;

            result = _context.SaveChanges() > 0;

            return result ? Response<bool>.Success() : Response<bool>.Error();
        }

        public async Task<IResponse<bool>> DeleteAsync(TEntity entity)
        {
            bool result = false;
            var deletedEntity = _context.Entry(entity);
            deletedEntity.State = EntityState.Deleted;

            result = await _context.SaveChangesAsync() > 0;

            return result ? Response<bool>.Success() : Response<bool>.Error();
        }

        public IResponse<IList<TEntity>> GetAll(Expression<Func<TEntity, bool>> predicate = null)
        {
            var data = predicate == null ?
                _entity.ToList()
                :
                _entity.Where(predicate).ToList();

            return Response<IList<TEntity>>.Success(data);
            
        }

        public async Task<IResponse<IList<TEntity>>> GetAllAsync(Expression<Func<TEntity, bool>> predicate = null)
        {
            var data = predicate == null ? await
                _entity.ToListAsync()
                :
                await _entity.Where(predicate).ToListAsync();

            return Response<IList<TEntity>>.Success(data);
        }

        public IResponse<TEntity> GetOne(Expression<Func<TEntity, bool>> predicate)
        {

            var data = _entity.Where(predicate).SingleOrDefault();

            return Response<TEntity>.Success(data);
            
        }

        public async Task<IResponse<TEntity>> GetOneAsync(Expression<Func<TEntity, bool>> predicate)
        {
            var data = await _entity.Where(predicate).SingleOrDefaultAsync();

            return Response<TEntity>.Success(data);
        }

        public IResponse<bool> Insert(TEntity entity)
        {
            bool result = false;

            var addedEntity = _context.Entry(entity);
            addedEntity.State = EntityState.Added;

            result = _context.SaveChanges() > 0;

            return result ? Response<bool>.Success() : Response<bool>.Error();
        }

        public async Task<IResponse<bool>> InsertAsync(TEntity entity)
        {
            bool result = false;

            var addedEntity = _context.Entry(entity);
            addedEntity.State = EntityState.Added;

            result = await _context.SaveChangesAsync() > 0;

            return result ? Response<bool>.Success() : Response<bool>.Error();
        }

        public IResponse<bool> Update(TEntity entity)
        {
            bool result = false;

            var updatedEntity = _context.Entry(entity);
            updatedEntity.State = EntityState.Modified;

            result = _context.SaveChanges() > 0;

            return result ? Response<bool>.Success() : Response<bool>.Error();
        }

        public async Task<IResponse<bool>> UpdateAsync(TEntity entity)
        {
            bool result = false;

            var updatedEntity = _context.Entry(entity);
            updatedEntity.State = EntityState.Modified;

            result = await _context.SaveChangesAsync() > 0;

            return result ? Response<bool>.Success() : Response<bool>.Error();
        }
    }
}
