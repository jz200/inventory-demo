using BBMDemo.Data.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace BBMDemo.Data.Services
{
    public class DbReadService: IDbReadService
    {
        private BBMContext _db;
        public DbReadService(BBMContext db)
        {
            _db = db;
        }

        public (int products, int categories, int styles, int locations, int users, int inventories) Count()
        {
            return (products: _db.Product.Count(),
                    categories: _db.Category.Count(),
                    styles: _db.Style.Count(),
                    locations: _db.Location.Count(),
                    users: _db.Users.Count(),
                    inventories: _db.ProductInventory.Count()
                    );
        }

        public IQueryable<TEntity> Get<TEntity>() where TEntity: class
        {
            return _db.Set<TEntity>();
        }

        public TEntity Get<TEntity>(int id, bool includeRelatedEntities = false) where TEntity : class
        {
            var record = _db.Set<TEntity>().Find(new object[] { id });
            if (record != null && includeRelatedEntities)
            {
                var entities = GetEntityNames<TEntity>();
                foreach (var entity in entities.collections)
                {
                    _db.Entry(record).Collection(entity).Load();
                }
                foreach (var entity in entities.references)
                {
                    _db.Entry(record).Reference(entity).Load();
                }
            }
            return record;
        }

        public TEntity Get<TEntity>(int productId, int styleId, int locationId, bool includeRelatedEntities=false) where TEntity : class
        {
            var record = _db.Set<TEntity>().Find(new object[] { productId, styleId, locationId });
            if (record != null && includeRelatedEntities)
            {
                var entities = GetEntityNames<TEntity>();
                foreach (var entity in entities.collections)
                {
                    _db.Entry(record).Collection(entity).Load();
                }
                foreach (var entity in entities.references)
                {
                    _db.Entry(record).Reference(entity).Load();
                }
            }
            return record;
        }

        public SelectList GetSelectList<TEntity>(string valueField, string textField) where TEntity : class
        {
            return new SelectList(_db.Set<TEntity>(), valueField, textField);
        }

        public IQueryable<TEntity> GetWithIncludes<TEntity>() where TEntity : class
        {
            var entityNames = GetEntityNames<TEntity>();
            var dbset = _db.Set<TEntity>();
            var entities = entityNames.collections.Union(entityNames.references);
            foreach (var entity in entities)
            {
                dbset.Include(entity).Load();
            }
            return dbset;
        }

        private (IEnumerable<string> collections, IEnumerable<string> references) GetEntityNames<TEntity>() where TEntity : class
        {
            var dbsets = typeof(BBMContext).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(z => z.PropertyType.Name.Contains("DbSet")).Select(z => z.Name);
            var properties = typeof(TEntity).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var collections = properties.Where(l => dbsets.Contains(l.Name) && l.PropertyType.IsGenericType).Select(s => s.Name);
            var classes = properties.Where(c => dbsets.Contains(c.Name) && !c.PropertyType.IsGenericType).Select(s => s.Name);
            return (collections: collections, references: classes);
        }
    }
}
