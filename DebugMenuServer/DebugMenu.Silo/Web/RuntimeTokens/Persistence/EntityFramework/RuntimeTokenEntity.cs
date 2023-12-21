using DebugMenu.Silo.Persistence;
using DebugMenu.Silo.Web.Applications.Persistence.EntityFramework;

namespace DebugMenu.Silo.Web.RuntimeTokens.Persistence.EntityFramework; 

public class RuntimeTokenEntity : EntityWithIntId {
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;

    public ApplicationEntity Application { get; set; } = null!;
}