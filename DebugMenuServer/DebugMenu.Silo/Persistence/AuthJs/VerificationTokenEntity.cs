namespace DebugMenu.Silo.Persistence.AuthJs;

public class VerificationTokenEntity {
    public string Identifier { get; set; } = null!;
    public DateTime Expires { get; set; }
    public string Token { get; set; } = null!;
}