using DebugMenu.Silo.Persistence.AuthJs;

namespace DebugMenu.Silo.Web.Applications.Persistence.EntityFramework;

public class ApplicationUserEntity {
    public int ApplicationId { get; set; }
    public ApplicationEntity Application { get; set; } = null!;
    public string UserId { get; set; }
    public UserEntity User { get; set; } = null!;

    public ApplicationMemberRole Role { get; set; } = ApplicationMemberRole.Member;
}
