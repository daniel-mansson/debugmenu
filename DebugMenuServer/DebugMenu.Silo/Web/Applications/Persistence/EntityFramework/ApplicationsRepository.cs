using DebugMenu.Silo.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DebugMenu.Silo.Web.Applications.Persistence.EntityFramework;

public class ApplicationsRepository : CrudRepositoryBase<ApplicationEntity, int>, IApplicationsRepository {
    private readonly DebugMenuDbContext _context;

    public ApplicationsRepository(DebugMenuDbContext context) : base(context) {
        _context = context;
    }

    protected override DbSet<ApplicationEntity> DbSet => _context.Applications;
    protected override IQueryable<ApplicationEntity> HydratedQueryable => DbSet
        .Include(u => u.Team)
        .Include(u => u.Team.Users);

    public async Task<IReadOnlyList<ApplicationEntity>> GetByTeamIdAsync(int teamId) {
        return await DbSet
            .Where(application => application.TeamId == teamId)
            .ToListAsync();
    }
}

