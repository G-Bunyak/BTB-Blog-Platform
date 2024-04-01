namespace BlogPlatform.Interfaces.DatabaseRepositories
{
    public interface IRepositoryBase<TEntity> where TEntity : class
    {
        Task<TEntity> InsertAsync(TEntity entity, bool saveChanges = true);
        Task<TEntity> UpdateAsync(TEntity entityToUpdate, bool disableChangeTracker = true, bool saveChanges = true);
        Task<List<TEntity>> GetAllItemsAsync();
    }
}
