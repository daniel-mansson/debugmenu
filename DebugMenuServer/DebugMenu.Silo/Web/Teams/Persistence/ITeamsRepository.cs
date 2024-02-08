using DebugMenu.Silo.Persistence;
using DebugMenu.Silo.Web.Teams.Persistence.EntityFramework;

namespace DebugMenu.Silo.Web.Teams.Persistence;

public interface ITeamsRepository : ICrudRepository<TeamEntity, int> {
    Task<IReadOnlyList<TeamEntity>> GetByUserIdAsync(string userId);
    Task<IReadOnlyList<TeamUserEntity>> GetUsersInTeamAsync(int teamId);
}
