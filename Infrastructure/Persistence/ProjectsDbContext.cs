using Domain.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class ProjectsDbContext(DbContextOptions<ProjectsDbContext> dbContextOptions) : IdentityDbContext<User,Role, Guid>(dbContextOptions)
{
    internal DbSet<Project> Projects { get; set; } = null!;
    internal DbSet<TeamMember> TeamMembers { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Unicité de l'email
        builder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        // Clé composite pour TeamMember
        builder.Entity<TeamMember>()
            .HasKey(tm => new { tm.ProjectId, tm.UserId, tm.RoleId })
            .HasName("PK_TeamMember");

        builder.Entity<TeamMember>()
            .HasOne(tm => tm.Project)
            .WithMany(p => p.TeamMembers)
            .HasForeignKey(tm => tm.ProjectId);

        builder.Entity<TeamMember>()
            .HasOne(tm => tm.User)
            .WithMany(u => u.TeamMembers)
            .HasForeignKey(tm => tm.UserId);

        builder.Entity<TeamMember>()
            .HasOne(tm => tm.Role)
            .WithMany(r => r.TeamMembers)
            .HasForeignKey(tm => tm.RoleId);

        builder.Entity<Project>()
            .HasMany(p => p.TeamMembers)
            .WithOne(tm => tm.Project)
            .HasForeignKey(tm => tm.ProjectId);

        builder.Entity<Project>()
            .HasOne(p => p.CreatedBy_User)
            .WithMany(u => u.Projects)
            .HasForeignKey(p => p.CreatedBy)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
