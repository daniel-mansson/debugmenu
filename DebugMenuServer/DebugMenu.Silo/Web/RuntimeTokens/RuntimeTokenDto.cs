using AutoMapper;
using DebugMenu.Silo.Web.RuntimeTokens.Persistence.EntityFramework;

namespace DebugMenu.Silo.Web.RuntimeTokens; 

[AutoMap(typeof(RuntimeTokenEntity), ReverseMap = true)]
public class RuntimeTokenDto {
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
}