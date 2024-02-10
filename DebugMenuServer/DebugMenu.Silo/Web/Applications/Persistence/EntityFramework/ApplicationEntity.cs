using System.ComponentModel.DataAnnotations;
using DebugMenu.Silo.Persistence;
using DebugMenu.Silo.Persistence.AuthJs;
using DebugMenu.Silo.Web.Teams.Persistence.EntityFramework;

namespace DebugMenu.Silo.Web.Applications.Persistence.EntityFramework;

public class ApplicationEntity : EntityWithIntId {
    public string Name { get; set; } = string.Empty;

    public int TeamId { get; set; }
    public TeamEntity Team { get; set; }
}
