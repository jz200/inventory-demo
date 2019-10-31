using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BBMDemo.Data.Services
{
    public interface IDbReadService
    {
        IQueryable<TEntity> Get<TEntity>() where TEntity : class;
        TEntity Get<TEntity>(int id, bool includeRelatedEntities = false)
            where TEntity : class;
        TEntity Get<TEntity>(int productId, int styleId, int locationId, bool includeRelatedEntities = false)
            where TEntity : class;
        IQueryable<TEntity> GetWithIncludes<TEntity>() where TEntity : class;
        SelectList GetSelectList<TEntity>(string valueField, string textField)
            where TEntity : class;
        (int products, int categories, int styles, int locations, int users,
            int inventories) Count();
    }
}
