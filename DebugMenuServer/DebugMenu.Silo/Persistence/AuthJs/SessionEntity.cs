namespace DebugMenu.Silo.Persistence.AuthJs;

public class SessionEntity : EntityWithIntId {
    public string AccountId { get; set; }
    public string UserId { get; set; }
    public DateTime Expires { get; set; }
}
