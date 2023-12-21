using AutoMapper;
using DebugMenu.Silo.Web.RuntimeTokens.Persistence.EntityFramework;

namespace DebugMenu.Silo.Web.RuntimeTokens; 

public class RuntimeTokenMutableDto {
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}