using AutoMapper;
using DebugMenu.Silo.Persistence.AuthJs;

namespace DebugMenu.Silo.Web.Users; 

[AutoMap(typeof(UserEntity), ReverseMap = true)]
public class UserDto {
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public DateTime? EmailVerified { get; set; }
    public string? Image { get; set; }
}