using DebugMenu.Silo.Persistence;
using DebugMenu.Silo.Persistence.AuthJs;

namespace DebugMenu.Silo.Web.Users.Persistence;

public interface IUserRepository : ICrudRepository<UserEntity, string> {

}
