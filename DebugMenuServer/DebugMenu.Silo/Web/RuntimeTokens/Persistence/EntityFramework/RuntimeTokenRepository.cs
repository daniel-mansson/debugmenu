using DebugMenu.Silo.Persistence;
using DebugMenu.Silo.Web.Applications.Persistence.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace DebugMenu.Silo.Web.RuntimeTokens.Persistence.EntityFramework;

public class RuntimeTokenRepository : CrudRepositoryBase<RuntimeTokenEntity, int>, IRuntimeTokenRepository {
    private readonly DebugMenuDbContext _context;

    public RuntimeTokenRepository(DebugMenuDbContext context) : base(context) {
        _context = context;
    }

    protected override DbSet<RuntimeTokenEntity> DbSet => _context.RuntimeTokens;

    public async Task<IReadOnlyList<RuntimeTokenEntity>> GetByApplicationAsync(int applicationId) {
        return await DbSet.Where(token => token.Application.Id == applicationId)
            .ToListAsync();
    }

    public async Task<RuntimeTokenEntity?> GetByTokenAsync(string token) {
        return await DbSet
            .Include(e => e.Application)
            .FirstOrDefaultAsync(entity => entity.Token == token);
    }
}
