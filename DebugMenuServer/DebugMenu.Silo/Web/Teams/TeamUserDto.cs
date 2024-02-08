using DebugMenu.Silo.Web.Teams.Persistence.EntityFramework;
using DebugMenu.Silo.Web.Users;

namespace DebugMenu.Silo.Web.Teams;

public class TeamUserDto {
    public string UserId { get; set; }
    public UserDto? User { get; set; }
    public TeamMemberRole Role { get; set; }
}
