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

    public virtual DbSet<SessionEntity> Sessions { get; set; } = null!;
    public virtual DbSet<UserEntity> Users { get; set; } = null!;


    public virtual DbSet<ApplicationEntity> Applications { get; set; } = null!;
    public virtual DbSet<TeamEntity> Teams { get; set; } = null!;
    public virtual DbSet<RuntimeTokenEntity> RuntimeTokens { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql(
            "Host=localhost;Database=debugmenu;Username=postgres;Password=postgres;Include Error Detail=true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.Entity<SessionEntity>(entity => {
            entity.HasKey(e => e.Id).HasName("sessions_pkey");

            entity.ToTable("sessions");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Expires).HasColumnName("expires");
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
