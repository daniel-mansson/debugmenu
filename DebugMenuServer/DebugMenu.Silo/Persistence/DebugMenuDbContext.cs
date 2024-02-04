using DebugMenu.Silo.Persistence.AuthJs;
using DebugMenu.Silo.Web.Applications.Persistence.EntityFramework;
using DebugMenu.Silo.Web.RuntimeTokens.Persistence.EntityFramework;
using DebugMenu.Silo.Web.Teams.Persistence.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace DebugMenu.Silo.Persistence;

public class DebugMenuDbContext : DbContext {
    public DebugMenuDbContext() {
    }

    public DebugMenuDbContext(DbContextOptions<DebugMenuDbContext> options)
        : base(options) {
    }

    public virtual DbSet<AccountEntity> Accounts { get; set; } = null!;

    public virtual DbSet<SessionEntity> Sessions { get; set; } = null!;

    public virtual DbSet<UserEntity> Users { get; set; } = null!;

    public virtual DbSet<VerificationTokenEntity> VerificationTokens { get; set; } = null!;

    public virtual DbSet<ApplicationEntity> Applications { get; set; } = null!;
    public virtual DbSet<TeamEntity> Teams { get; set; } = null!;
    public virtual DbSet<RuntimeTokenEntity> RuntimeTokens { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql(
            "Host=localhost;Database=debugmenu2;Username=postgres;Password=postgres;Include Error Detail=true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.Entity<AccountEntity>(entity => {
            entity.HasKey(e => e.Id).HasName("accounts_pkey");

            entity.ToTable("accounts");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AccessToken).HasColumnName("access_token");
            entity.Property(e => e.ExpiresAt).HasColumnName("expires_at");
            entity.Property(e => e.IdToken).HasColumnName("id_token");
            entity.Property(e => e.Provider)
                .HasMaxLength(255)
                .HasColumnName("provider");
            entity.Property(e => e.ProviderAccountId)
                .HasMaxLength(255)
                .HasColumnName("providerAccountId");
            entity.Property(e => e.RefreshToken).HasColumnName("refresh_token");
            entity.Property(e => e.Scope).HasColumnName("scope");
            entity.Property(e => e.SessionState).HasColumnName("session_state");
            entity.Property(e => e.TokenType).HasColumnName("token_type");
            entity.Property(e => e.Type)
                .HasMaxLength(255)
                .HasColumnName("type");
            entity.Property(e => e.UserId).HasColumnName("userId");
        });

        modelBuilder.Entity<SessionEntity>(entity => {
            entity.HasKey(e => e.Id).HasName("sessions_pkey");

            entity.ToTable("sessions");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Expires).HasColumnName("expires");
            entity.Property(e => e.SessionToken)
                .HasMaxLength(255)
                .HasColumnName("sessionToken");
            entity.Property(e => e.UserId).HasColumnName("userId");
        });

        modelBuilder.Entity<UserEntity>(entity => {
            entity.HasKey(e => e.Id).HasName("users_pkey");

            entity.ToTable("users");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.EmailVerified).HasColumnName("emailVerified");
            entity.Property(e => e.Image).HasColumnName("image");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
        });

        modelBuilder.Entity<VerificationTokenEntity>(entity => {
            entity.HasKey(e => new { e.Identifier, e.Token }).HasName("verification_token_pkey");

            entity.ToTable("verification_token");

            entity.Property(e => e.Identifier).HasColumnName("identifier");
            entity.Property(e => e.Token).HasColumnName("token");
            entity.Property(e => e.Expires).HasColumnName("expires");
        });

        modelBuilder.Entity<ApplicationEntity>(entity => {
            entity.ToTable("applications");

            modelBuilder.Entity<ApplicationEntity>()
                .HasMany(e => e.Users)
                .WithMany(e => e.Applications)
                .UsingEntity<ApplicationUserEntity>(
                    j => j.Property(e => e.Role).HasDefaultValue(ApplicationMemberRole.Member));
        });

        modelBuilder.Entity<ApplicationUserEntity>(entity => {
            entity.ToTable("applications_users");
        });

        modelBuilder.Entity<TeamEntity>(entity => {
            entity.ToTable("teams");

            modelBuilder.Entity<TeamEntity>()
                .HasMany(e => e.Users)
                .WithMany(e => e.Teams)
                .UsingEntity<TeamUserEntity>(
                    j => j.Property(e => e.Role).HasDefaultValue(TeamMemberRole.Member));
        });

        modelBuilder.Entity<TeamUserEntity>(entity => {
            entity.ToTable("teams_users");
        });

        modelBuilder.Entity<RuntimeTokenEntity>(entity => {
            entity.HasOne<ApplicationEntity>();
            entity.ToTable("runtime_tokens");
        });
    }
}
