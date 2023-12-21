using DebugMenu.Silo.Web.Applications.Persistence.EntityFramework;

namespace DebugMenu.Silo.Persistence.AuthJs;

public class UserEntity : EntityWithIntId {
    public string? Name { get; set; }
    public string? Email { get; set; }
    public DateTime? EmailVerified { get; set; }
    public string? Image { get; set; }

    public ICollection<ApplicationEntity> Applications { get; set; } = new List<ApplicationEntity>();
    public ICollection<ApplicationUserEntity> ApplicationUsers { get; set; } = new List<ApplicationUserEntity>();
}