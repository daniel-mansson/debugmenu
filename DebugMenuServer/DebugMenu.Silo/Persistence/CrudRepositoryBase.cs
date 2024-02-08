using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace DebugMenu.Silo.Persistence;

public abstract class CrudRepositoryBase<TEntity, TKey> : ICrudRepository<TEntity, TKey> where TEntity : EntityWithId<TKey> {
    private readonly DbContext _context;

    protected abstract DbSet<TEntity> DbSet { get; }
    protected virtual IQueryable<TEntity> HydratedQueryable => DbSet;

    protected CrudRepositoryBase(DbContext context) {
        _context = context;
    }

    public TEntity Create(TEntity entity) {
        if (entity == null) {
            throw new ArgumentNullException(nameof(entity));
        }

        EntityEntry<TEntity> entry = _context.Add(entity);
        return entry.Entity;
    }

    public void Update(TEntity item) {
        var entry = DbSet.Entry(item);
        if (entry.State == EntityState.Detached) {
            DbSet.Attach(item);
        }

        entry.State = EntityState.Modified;
    }

    public async Task<IReadOnlyList<TEntity>> GetAsync(Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null) {
        IQueryable<TEntity> query = HydratedQueryable;

        if (filter != null) {
            query = query.Where(filter);
        }

        if (orderBy != null) {
            return await orderBy(query).ToListAsync();
        }

        return await query.ToListAsync();
    }

    public Task<TEntity?> GetByIdAsync(TKey id) {
        return HydratedQueryable.FirstOrDefaultAsync(e => e.Id.Equals(id));
    }

    public void Delete(TEntity entity) {
        var entry = DbSet.Entry(entity);
        if (entry.State == EntityState.Detached) {
            DbSet.Attach(entity);
        }

        entry.State = EntityState.Deleted;
    }

    public async Task DeleteAsync(TKey id) {
        var entity = await DbSet.FindAsync(id);
        if (entity == null) {
            return;
        }

        var entry = DbSet.Entry(entity);
        if (entry.State == EntityState.Detached) {
            DbSet.Attach(entity);
        }

        entry.State = EntityState.Deleted;
    }

    public Task SaveAsync() {
        return _context.SaveChangesAsync();
    }
}
