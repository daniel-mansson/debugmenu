using DebugMenu.Silo.Persistence;
using DebugMenu.Silo.Web.Teams.Persistence.EntityFramework;

namespace DebugMenu.Silo.Web.Teams.Persistence;

public interface ITeamsRepository : ICrudRepository<TeamEntity> {
    Task<IReadOnlyList<TeamEntity>> GetByUserIdAsync(int userId);
    Task<IReadOnlyList<TeamUserEntity>> GetUsersInTeamAsync(int teamId);
}
