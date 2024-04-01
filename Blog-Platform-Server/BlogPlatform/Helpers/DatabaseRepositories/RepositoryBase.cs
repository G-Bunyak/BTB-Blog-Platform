#region Imports
using BlogPlatform.Interfaces.DatabaseRepositories;
using Microsoft.EntityFrameworkCore;
#endregion

namespace BlogPlatform.Helpers.DatabaseRepositories
{
    public class RepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : class
    {
        #region Properties
        private readonly ApplicationContext context;
        private readonly DbSet<TEntity> dbSet;
        private readonly int connectionTimeoutValue = 180;
        #endregion

        #region Constructors
        public RepositoryBase(ApplicationContext context)
        {
            this.context = context;
            this.context.Database.SetCommandTimeout(connectionTimeoutValue);
            this.dbSet = context.Set<TEntity>();
        }
        #endregion

        #region Virtual Methods
        public virtual async Task<TEntity> InsertAsync(TEntity entity, bool saveChanges = true)
        {
            await context.Set<TEntity>().AddAsync(entity);

            if (saveChanges)
            {
                await context.SaveChangesAsync();
            }

            return entity;
        }

        public virtual async Task<TEntity> UpdateAsync(TEntity entityToUpdate, bool disableChangeTracker, bool saveChanges = true)
        {
            var changeTrackerStatus = context.ChangeTracker.AutoDetectChangesEnabled;

            if (disableChangeTracker)
            {
                context.ChangeTracker.AutoDetectChangesEnabled = false;
            }
            else
            {
                context.ChangeTracker.AutoDetectChangesEnabled = true;
            }

            if (context.Entry(entityToUpdate).State == EntityState.Detached)
            {
                dbSet.Attach(entityToUpdate);
            }

            context.Entry(entityToUpdate).State = EntityState.Modified;

            if (saveChanges)
            {
                await context.SaveChangesAsync();
            }

            context.ChangeTracker.AutoDetectChangesEnabled = changeTrackerStatus;
            return entityToUpdate;
        }

        public virtual async Task<List<TEntity>> GetAllItemsAsync()
        {         
            return await context.Set<TEntity>().Select(x => x).ToListAsync();
        }
        #endregion
    }
}
