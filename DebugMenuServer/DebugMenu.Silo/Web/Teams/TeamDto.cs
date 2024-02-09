using AutoMapper;
using DebugMenu.Silo.Web.Teams.Persistence.EntityFramework;
using Newtonsoft.Json;

namespace DebugMenu.Silo.Web.Teams;

[AutoMap(typeof(TeamEntity), ReverseMap = true)]
public class TeamDto {
    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Include)]
    public int Id { get; set; }
    public TeamType Type { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Icon { get; set; }
}
