using System.ComponentModel.DataAnnotations;
using DebugMenu.Silo.Persistence;
using DebugMenu.Silo.Persistence.AuthJs;

namespace DebugMenu.Silo.Web.Applications.Persistence.EntityFramework; 

public class ApplicationEntity : EntityWithIntId {
    public string Name { get; set; } = string.Empty;

    public ICollection<UserEntity> Users { get; set; } = new List<UserEntity>();
    public ICollection<ApplicationUserEntity> ApplicationUsers { get; set; } = new List<ApplicationUserEntity>();
}