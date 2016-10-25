using Notes.Common.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Notes.Common.Repositories
{
    public abstract class BaseRepository<T> : IDisposable where T : BaseEntity
    {
        internal NoteDbContext db;

        internal BaseRepository()
        {
            db = new NoteDbContext();
        }

        internal BaseRepository(NoteDbContext context)
        {
            db = context;
        }

        public virtual IQueryable<T> Get()
        {
            return db.Set<T>();
        }

        public virtual async Task<T> Get(int id)
        {
            return await db.Set<T>().FindAsync(id);
        }

        public virtual IQueryable<T> Get(bool? isActive)
        {
            return Get().Where(e => !isActive.HasValue || e.IsActive == isActive.Value);
        }

        public virtual IQueryable<T> GetActiveEntities()
        {
            return Get().Where(e => e.IsActive);
        }

        public virtual IQueryable<T> Search(string text)
        {
            IQueryable<T> query = Get();
            if (string.IsNullOrWhiteSpace(text))
            {
                return query;
            }

            var props = typeof(T).GetProperties().Where(p => p.PropertyType == typeof(string));
            if (!props.Any())
            {
                return query;
            }

            text = text.Trim();
            var arg = Expression.Parameter(typeof(T));
            Expression expr = Expression.Constant(false);
            foreach (var prop in props)
            {
                expr = Expression.OrElse(expr,
                    Expression.Call(Expression.Property(arg, prop),
                    typeof(string).GetMethod("Contains", new Type[] { typeof(string) }),
                    Expression.Constant(text)));
            }
            query = query.Where(Expression.Lambda<Func<T, bool>>(expr, arg));
            return query;
        }

        public virtual async Task Update(int id, T entity)
        {
            if (id != entity.Id)
            {
                throw new Exception("Id is not same");
            }

            entity.ModifiedAt = DateTimeOffset.Now;
            db.Entry(entity).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IsEntityExists(id))
                {
                    throw new Exception("Entity doesn't exist");
                }
                else
                {
                    throw;
                }
            }
        }

        public virtual async Task<T> Insert(T entity)
        {
            var insert = AddEntity(entity);
            await db.SaveChangesAsync();
            return insert;
        }

        public virtual async Task<IEnumerable<T>> InsertMany(IEnumerable<T> entities)
        {
            var inserts = new List<T>();
            foreach (var entity in entities)
            {
                inserts.Add(AddEntity(entity));
            }
            await db.SaveChangesAsync();
            return inserts;
        }

        public virtual async Task<T> Delete(int id)
        {
            T entity = await db.Set<T>().FindAsync(id);
            if (entity == null)
            {
                throw new Exception("Not Found");
            }

            T delete = db.Set<T>().Remove(entity);
            await db.SaveChangesAsync();
            return delete;
        }

        protected virtual bool IsEntityExists(int id)
        {
            return db.Set<T>().Count(e => e.Id == id) > 0;
        }

        public void Dispose()
        {
            if (db != null)
            {
                db.Dispose();
            }
        }

        private T AddEntity(T entity)
        {
            entity.CreatedAt = DateTimeOffset.Now;
            return db.Set<T>().Add(entity);
        }
    }
}
