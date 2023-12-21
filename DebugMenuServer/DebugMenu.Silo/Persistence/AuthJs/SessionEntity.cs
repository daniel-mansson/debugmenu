namespace DebugMenu.Silo.Persistence.AuthJs;

public class SessionEntity : EntityWithIntId {
    public int UserId { get; set; }
    public DateTime Expires { get; set; }
    public string SessionToken { get; set; } = null!;
}