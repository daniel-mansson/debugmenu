using DebugMenu.Silo.Persistence;
using DebugMenu.Silo.Web.Applications.Persistence.EntityFramework;

namespace DebugMenu.Silo.Web.Applications.Persistence;

public interface IApplicationsRepository: ICrudRepository<ApplicationEntity, int> {
    Task<IReadOnlyList<ApplicationEntity>> GetByTeamIdAsync(int teamId);
}
