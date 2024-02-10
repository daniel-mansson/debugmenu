using DebugMenu.Silo.Persistence;
using DebugMenu.Silo.Persistence.AuthJs;
using DebugMenu.Silo.Web.Applications.Persistence.EntityFramework;

namespace DebugMenu.Silo.Web.Teams.Persistence.EntityFramework;

public class TeamEntity : EntityWithIntId {
    public TeamType Type { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Icon { get; set; }

    public ICollection<ApplicationEntity> Applications { get; set; } = new List<ApplicationEntity>();
    public ICollection<UserEntity> Users { get; set; } = new List<UserEntity>();
    public ICollection<TeamUserEntity> TeamUsers { get; set; } = new List<TeamUserEntity>();
}
