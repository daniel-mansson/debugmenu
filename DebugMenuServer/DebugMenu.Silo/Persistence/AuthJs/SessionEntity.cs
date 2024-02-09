namespace DebugMenu.Silo.Persistence.AuthJs;

public class SessionEntity : EntityWithId<string> {
    public string UserId { get; set; }
    public DateTime ExpiresAt { get; set; }
}
