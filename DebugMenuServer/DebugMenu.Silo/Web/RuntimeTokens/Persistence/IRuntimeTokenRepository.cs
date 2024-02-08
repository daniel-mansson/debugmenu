using DebugMenu.Silo.Persistence;
using DebugMenu.Silo.Web.Applications.Persistence.EntityFramework;
using DebugMenu.Silo.Web.RuntimeTokens.Persistence.EntityFramework;

namespace DebugMenu.Silo.Web.RuntimeTokens.Persistence;

public interface IRuntimeTokenRepository : ICrudRepository<RuntimeTokenEntity, int> {
    public Task<IReadOnlyList<RuntimeTokenEntity>> GetByApplicationAsync(int applicationId);
    public Task<RuntimeTokenEntity?> GetByTokenAsync(string token);
}
