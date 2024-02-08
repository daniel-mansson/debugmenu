using AutoMapper;
using DebugMenu.Silo.Web.Applications.Persistence.EntityFramework;
using DebugMenu.Silo.Web.Users;

namespace DebugMenu.Silo.Web.Applications;

[AutoMap(typeof(ApplicationUserEntity), ReverseMap = true)]
public class ApplicationUserDto {
    public string UserId { get; set; }
    public UserDto? User { get; set; }
    public ApplicationMemberRole Role { get; set; }
}
