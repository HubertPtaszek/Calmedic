using Microsoft.EntityFrameworkCore;
using Calmedic.Domain;
using Calmedic.EntityFramework;
using Calmedic.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Calmedic.Data
{
    public class Repository<TEntity, TDbContext> : IRepository<TEntity> where TEntity : class where TDbContext : DbContext
    {
        protected readonly DbSet<TEntity> _dbset;
        protected readonly TDbContext Context;
 
        public MainContext MainContext { get; set; }

        public Repository(TDbContext context)
        {
            Context = context;
            _dbset = Context.Set<TEntity>();
        }

        public virtual void Add(TEntity entity)
        {
            if (entity is IAuditEntity)
            {
                var auditObject = entity as IAuditEntity;
                auditObject.CreatedById = MainContext.PersonId;

                if (auditObject.CreatedDate == DateTime.MinValue)
                {
                    auditObject.CreatedDate = DateTime.Now;
                }
            }
            _dbset.Add(entity);
        }
        public virtual void AddRange(List<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                if (entity is IAuditEntity)
                {
                    var auditObject = entity as IAuditEntity;
                    auditObject.CreatedById = MainContext.PersonId;

                    if (auditObject.CreatedDate == DateTime.MinValue)
                    {
                        auditObject.CreatedDate = DateTime.Now;
                    }
                }
                _dbset.Add(entity);
            }
        }

        public void Delete(TEntity entity)
        {
            Context.Entry(entity).State = EntityState.Deleted;
            _dbset.Remove(entity);
        }

        public void DeleteWhere(Expression<Func<TEntity, bool>> whereCondition)
        {
            Context.Set<TEntity>().RemoveRange(Context.Set<TEntity>().Where(whereCondition));
        }

        protected IQueryable<TEntity> GetQueryable()
        {
            return _dbset.AsQueryable();
        }

        public IList<TEntity> GetAll()
        {
            return _dbset.ToList();
        }

        public async Task<List<TEntity>> GetAllAsync()
        {
            return await _dbset.ToListAsync();
        }


        public IList<TEntity> GetAll(Expression<Func<TEntity, bool>> whereCondition)
        {
            return _dbset.Where(whereCondition).ToList();
        }

        public async Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> whereCondition)
        {
            return await _dbset.Where(whereCondition).ToListAsync();
        }

        public TEntity GetSingle(Expression<Func<TEntity, bool>> whereCondition)
        {
            return _dbset.Where(whereCondition).FirstOrDefault();
        }

        public async Task<TEntity> GetSingleAsync(Expression<Func<TEntity, bool>> whereCondition)
        {
            return await _dbset.Where(whereCondition).FirstOrDefaultAsync();
        }

        public void Attach(TEntity entity)
        {
            _dbset.Attach(entity);
        }

        public void Detach(TEntity entity)
        {
            Context.Entry(entity).State = EntityState.Detached;
        }

        public IQueryable<TEntity> GetQueryable(bool tracking = false)
        {
            if (tracking == false)
            {
                return _dbset.AsNoTracking();
            }
            return _dbset.AsQueryable();
        }

        public IQueryable<TEntity> GetQueryable(Expression<Func<TEntity, bool>> whereCondition)
        {
            return _dbset.AsQueryable().Where(whereCondition);
        }

        public int Count()
        {
            return _dbset.Count();
        }

        public async Task<int> CountAsync()
        {
            return await _dbset.CountAsync();
        }

        public bool Any()
        {
            return _dbset.Any();
        }

        public async Task<bool> AnyAsync()
        {
            return await _dbset.AnyAsync();
        }

        public async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> whereCondition)
        {
            return await _dbset.FirstOrDefaultAsync(whereCondition);
        }

        public int Count(Expression<Func<TEntity, bool>> whereCondition)
        {
            return _dbset.Where(whereCondition).Count();
        }

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>> whereCondition)
        {
            return await _dbset.Where(whereCondition).CountAsync();
        }

        public bool Any(Expression<Func<TEntity, bool>> whereCondition)
        {
            return _dbset.Any(whereCondition);
        }

        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> whereCondition)
        {
            return await _dbset.AnyAsync(whereCondition);
        }

        public void Edit(TEntity entity)
        {
            if (Context.Entry(entity).State != EntityState.Added)
            {
                Context.Entry(entity).State = EntityState.Modified;
                if (entity is IAuditEntity)
                {
                    var auditObject = entity as IAuditEntity;
                    auditObject.ModifiedById = MainContext.PersonId;
                    auditObject.ModifiedDate = DateTime.Now;
                }
            }
        }

        public void EditRange(List<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                Edit(entity);
            }
        }

        public void AddOrEdit<TEn>(TEn entity) where TEn : Entity, TEntity
        {
            if (entity.Id == 0 || entity.Id == -1)
            {
                Add(entity);
            }
            else
            {
                Edit(entity);
            }
        }

        public int Save()
        {
            return Context.SaveChanges();
        }

        public async Task<int> SaveAsync()
        {
            return await Context.SaveChangesAsync();
        }

        public GetDataResult<TResult> GetData<TResult>(DxGridParams gridParams, IQueryable<TResult> query)
        {
            //see the QueryHelper class for the implementation
            GetDataResult<TResult> result = new GetDataResult<TResult>();
            query = query.FilterByOptions(gridParams);   //filtering

            result.TotalCount = query.LongCount();

            query = query.SortByOptions(gridParams)     //sorting
              .PageByOptions(gridParams);    //paging

            result.ItemList = query.ToList<TResult>();

            return result;
        }

        public GetDataResult GetData(DxGridParams gridParams, IQueryable query)
        {
            IQueryable<dynamic> queryOb = query as IQueryable<dynamic>;
            //see the QueryHelper class for the implementation
            GetDataResult result = new GetDataResult();
            queryOb = queryOb.FilterByOptions<dynamic>(gridParams);   //filtering

            result.TotalCount = queryOb.LongCount();

            queryOb = queryOb.SortByOptions(gridParams)     //sorting
              .PageByOptions(gridParams);    //paging

            result.ItemList = queryOb.ToList<dynamic>();

            return result;
        }

        public GetDataResult GetData(DxGridParams gridParams, IQueryable<dynamic> query)
        {
            //see the QueryHelper class for the implementation
            GetDataResult result = new GetDataResult();
            query = query.FilterByOptions<dynamic>(gridParams);   //filtering

            result.TotalCount = query.LongCount();

            query = query.SortByOptions(gridParams)     //sorting
              .PageByOptions(gridParams);    //paging

            result.ItemList = query.ToList<object>();

            return result;
        }
        public TEntity Find(int id)
        {
            return _dbset.Find(id);
        }

    }

    public class Repository : IRepository
    {
        protected readonly MainDatabaseContext Context;
     
        public MainContext MainContext { get; set; }
        public Repository(MainDatabaseContext context)
        {
            Context = context;
        }

        public GetDataResult GetData(DxGridParams gridParams, IQueryable query)
        {
            IQueryable<dynamic> queryOb = query as IQueryable<dynamic>;
            //see the QueryHelper class for the implementation
            GetDataResult result = new GetDataResult();
            queryOb = queryOb.FilterByOptions<dynamic>(gridParams);   //filtering

            result.TotalCount = queryOb.LongCount();

            queryOb = queryOb.SortByOptions(gridParams)     //sorting
              .PageByOptions(gridParams);    //paging

            result.ItemList = queryOb.ToList<dynamic>();

            return result;
        }

        public GetDataResult GetData(DxGridParams gridParams, IQueryable<dynamic> query)
        {
            //see the QueryHelper class for the implementation
            GetDataResult result = new GetDataResult();
            query = query.FilterByOptions<dynamic>(gridParams);   //filtering

            result.TotalCount = query.LongCount();

            query = query.SortByOptions(gridParams)     //sorting
              .PageByOptions(gridParams);    //paging

            result.ItemList = query.ToList<object>();

            return result;
        }
    }

    public class Repository<TDbContext> : IRepository where TDbContext : DbContext
    {
        protected readonly TDbContext Context;

        public MainContext MainContext { get; set; }
        public Repository(TDbContext context)
        {
            Context = context;
        }
        public GetDataResult<TResult> GetData<TResult>(DxGridParams gridParams, IQueryable<TResult> query)
        {
            //see the QueryHelper class for the implementation
            GetDataResult<TResult> result = new GetDataResult<TResult>();
            query = query.FilterByOptions(gridParams);   //filtering

            result.TotalCount = query.LongCount();

            query = query.SortByOptions(gridParams)     //sorting
              .PageByOptions(gridParams);    //paging

            result.ItemList = query.ToList<TResult>();

            return result;
        }

        public GetDataResult GetData(DxGridParams gridParams, IQueryable query)
        {
            IQueryable<dynamic> queryOb = query as IQueryable<dynamic>;
            //see the QueryHelper class for the implementation
            GetDataResult result = new GetDataResult();
            queryOb = queryOb.FilterByOptions<dynamic>(gridParams);   //filtering

            result.TotalCount = queryOb.LongCount();

            queryOb = queryOb.SortByOptions(gridParams)     //sorting
              .PageByOptions(gridParams);    //paging

            result.ItemList = queryOb.ToList<dynamic>();

            return result;
        }

        public GetDataResult GetData(DxGridParams gridParams, IQueryable<dynamic> query)
        {
            //see the QueryHelper class for the implementation
            GetDataResult result = new GetDataResult();
            query = query.FilterByOptions<dynamic>(gridParams);   //filtering

            result.TotalCount = query.LongCount();

            query = query.SortByOptions(gridParams)     //sorting
              .PageByOptions(gridParams);    //paging

            result.ItemList = query.ToList<object>();

            return result;
        }

        public void Edit(object entity)
        {
            if (Context.Entry(entity).State != EntityState.Added)
            {
                Context.Entry(entity).State = EntityState.Modified;
            }
        }
    }
}
