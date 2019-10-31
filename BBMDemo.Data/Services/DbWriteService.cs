using BBMDemo.Data.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BBMDemo.Data.Services
{
    public class DbWriteService : IDbWriteService
    {
        private BBMContext _db;

        public DbWriteService(BBMContext db)
        {
            _db = db;
        }

        public async Task<bool> Add<TEntity>(TEntity item) where TEntity : class
        {

            await _db.AddAsync(item);
            //await _db.Set<TEntity>().AddAsync(item);
            int result = await _db.SaveChangesAsync();
            return result >= 0;                       
        }

        public async Task<bool> Delete<TEntity>(TEntity item) where TEntity : class
        {
            _db.Remove(item);
            //_db.Set<TEntity>().Remove(item);
            int result = await _db.SaveChangesAsync();
            return result >= 0;
            
        }

        public async Task<bool> Update<TEntity>(TEntity item) where TEntity : class
        {
                 _db.Update(item);
                //_db.Set<TEntity>().Update(item);
                return await _db.SaveChangesAsync() >= 0;
            }
        }
    
}
