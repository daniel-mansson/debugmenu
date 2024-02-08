using DebugMenu.Silo.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DebugMenu.Silo.Web.Teams.Persistence.EntityFramework;

public class TeamsRepository : CrudRepositoryBase<TeamEntity, int>, ITeamsRepository {
    private readonly DebugMenuDbContext _context;

    public TeamsRepository(DebugMenuDbContext context) : base(context) {
        _context = context;
    }

    protected override DbSet<TeamEntity> DbSet => _context.Teams;
    protected override IQueryable<TeamEntity> HydratedQueryable => DbSet.Include(u => u.Users);

    public async Task<IReadOnlyList<TeamEntity>> GetByUserIdAsync(string userId) {
        return await DbSet
            .Where(team => team.Users
                .Any(user => user.Id == userId))
            .ToListAsync();
    }

    public async Task<IReadOnlyList<TeamUserEntity>> GetUsersInTeamAsync(int teamId) {
        var team = await DbSet
            .Include(team => team.TeamUsers)
            .Include(team => team.Users)
            .FirstAsync(team => team.Id == teamId);

        return team.TeamUsers.ToList();
    }
}
