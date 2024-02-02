using AutoMapper;
using DebugMenu.Silo.Web.Teams.Persistence.EntityFramework;

namespace DebugMenu.Silo.Web.Teams;

[AutoMap(typeof(TeamEntity), ReverseMap = true)]
public class TeamDto {
    public TeamType Type { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Icon { get; set; }
}
