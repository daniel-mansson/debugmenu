using DebugMenu.Silo.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DebugMenu.Silo.Web.Applications.Persistence.EntityFramework;

public class ApplicationsRepository : CrudRepositoryBase<ApplicationEntity, int>, IApplicationsRepository {
    private readonly DebugMenuDbContext _context;

    public ApplicationsRepository(DebugMenuDbContext context) : base(context) {
        _context = context;
    }

    protected override DbSet<ApplicationEntity> DbSet => _context.Applications;
    protected override IQueryable<ApplicationEntity> HydratedQueryable => DbSet.Include(u => u.Users);

    public async Task<IReadOnlyList<ApplicationEntity>> GetByUserIdAsync(string userId) {
        return await DbSet
            .Where(application => application.Users
                .Any(user => user.Id == userId))
            .ToListAsync();
    }

    public async Task<IReadOnlyList<ApplicationUserEntity>> GetUsersInApplicationAsync(int applicationId) {
        var application = await DbSet
            .Include(application => application.ApplicationUsers)
            .Include(application => application.Users)
            .FirstAsync(application => application.Id == applicationId);

        return application.ApplicationUsers.ToList();
    }
}

