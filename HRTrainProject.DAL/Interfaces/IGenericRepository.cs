using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace HRTrainProject.Services.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> GetWithRawSql(string query, params object[] parameters);

        IQueryable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>,
            IOrderedQueryable<TEntity>> orderBy = null,
             string includeProperties = "");

        TEntity GetByID(object id);
        void Insert(TEntity entity);

        void Update(TEntity entity);
        void Delete(object id);
        void Delete(TEntity entity);
    }
}
