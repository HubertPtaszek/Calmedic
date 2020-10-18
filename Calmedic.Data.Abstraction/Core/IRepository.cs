using Calmedic.Domain;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Calmedic.Data
{
    public interface IRepository
    { }

    public interface IRepository<TEntity> where TEntity : class
    {
        Task<TEntity> GetSingleAsync(Expression<Func<TEntity, bool>> whereCondition);
        void Add(TEntity entity);
        void AddRange(List<TEntity> entities);
        void Delete(TEntity entity);
        void DeleteWhere(Expression<Func<TEntity, bool>> whereCondition);
        void Attach(TEntity entity);
        void Detach(TEntity entity);
        IList<TEntity> GetAll(Expression<Func<TEntity, bool>> whereCondition);
        IList<TEntity> GetAll();
        int Count(Expression<Func<TEntity, bool>> whereCondition);
        int Count();
        bool Any(Expression<Func<TEntity, bool>> whereCondition);
        bool Any();

        void AddOrEdit<TEn>(TEn entity) where TEn : Entity, TEntity;
        TEntity GetSingle(Expression<Func<TEntity, bool>> whereCondition);
        void Edit(TEntity entity);
        void EditRange(List<TEntity> entities);

        int Save();
        Task<int> SaveAsync();
        TEntity Find(int id);
    }
}
