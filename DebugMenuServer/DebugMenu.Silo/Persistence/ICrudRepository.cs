using System.Linq.Expressions;

namespace DebugMenu.Silo.Persistence;

public interface ICrudRepository<TEntity> : ICrudRepository<int, TEntity> {
}

public interface ICrudRepository<in TKey, TEntity> {
    TEntity Create(TEntity entity);
    void Update(TEntity entity);
    Task<IReadOnlyList<TEntity>> GetAsync(Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null);
    Task<TEntity?> GetByIdAsync(TKey id);
    void Delete(TEntity entity);
    Task DeleteAsync(int id);

    Task SaveAsync();
}