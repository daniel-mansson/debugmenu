using AutoMapper;
using DebugMenu.Silo.Web.Applications.Persistence.EntityFramework;

namespace DebugMenu.Silo.Web.Applications; 

[AutoMap(typeof(ApplicationEntity), ReverseMap = true)]
public class ApplicationDto {
    public int Id { get; set; }
    public string Name { get; set; }
}