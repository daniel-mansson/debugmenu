using DebugMenu.Silo.Persistence;
using DebugMenu.Silo.Persistence.AuthJs;
using Microsoft.EntityFrameworkCore;

namespace DebugMenu.Silo.Web.Users.Persistence.EntityFramework; 

public class UserRepository : CrudRepositoryBase<UserEntity>, IUserRepository {
    private readonly DebugMenuDbContext _context;

    public UserRepository(DebugMenuDbContext context) : base(context) {
        _context = context;
    }

    protected override DbSet<UserEntity> DbSet => _context.Users;
}