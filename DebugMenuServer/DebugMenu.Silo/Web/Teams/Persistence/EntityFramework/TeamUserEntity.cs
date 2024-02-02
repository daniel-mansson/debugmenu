using DebugMenu.Silo.Persistence.AuthJs;

namespace DebugMenu.Silo.Web.Teams.Persistence.EntityFramework;

public class TeamUserEntity {
    public int TeamId { get; set; }
    public TeamEntity Team { get; set; } = null!;
    public int UserId { get; set; }
    public UserEntity User { get; set; } = null!;

    public TeamMemberRole Role { get; set; } = TeamMemberRole.Member;
}
