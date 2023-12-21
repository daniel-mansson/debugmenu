using DebugMenu.Silo.Persistence;
using DebugMenu.Silo.Web.Applications.Persistence.EntityFramework;

namespace DebugMenu.Silo.Web.Applications.Persistence; 

public interface IApplicationsRepository: ICrudRepository<ApplicationEntity> {
    Task<IReadOnlyList<ApplicationEntity>> GetByUserIdAsync(int userId);
    Task<IReadOnlyList<ApplicationUserEntity>> GetUsersInApplicationAsync(int applicationId);
}