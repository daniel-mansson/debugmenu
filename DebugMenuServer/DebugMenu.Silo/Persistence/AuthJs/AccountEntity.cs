﻿namespace DebugMenu.Silo.Persistence.AuthJs;

public class AccountEntity : EntityWithIntId {
    public int UserId { get; set; }
    public string Type { get; set; } = null!;
    public string Provider { get; set; } = null!;
    public string ProviderAccountId { get; set; } = null!;
    public string? RefreshToken { get; set; }
    public string? AccessToken { get; set; }
    public long? ExpiresAt { get; set; }
    public string? IdToken { get; set; }
    public string? Scope { get; set; }
    public string? SessionState { get; set; }
    public string? TokenType { get; set; }
}