using Application.Core.Wrappers;
using Domain.Shared.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Shared.Repositories
{
    public interface IEntityRepository<TEntity> where TEntity: class, IEntity, new ()
    {
        IResponse<bool> Insert(TEntity entity);

        IResponse<bool> Update(TEntity entity);

        IResponse<bool> Delete(TEntity entity);

        IResponse<IList<TEntity>> GetAll(Expression<Func<TEntity, bool>> predicate = null);

        IResponse<TEntity> GetOne(Expression<Func<TEntity, bool>> predicate);



        Task<IResponse<bool>> InsertAsync(TEntity entity);

        Task<IResponse<bool>> UpdateAsync(TEntity entity);

        Task<IResponse<bool>> DeleteAsync(TEntity entity);

        Task<IResponse<IList<TEntity>>> GetAllAsync(Expression<Func<TEntity, bool>> predicate = null);

        Task<IResponse<TEntity>> GetOneAsync(Expression<Func<TEntity, bool>> predicate);
    }
}
