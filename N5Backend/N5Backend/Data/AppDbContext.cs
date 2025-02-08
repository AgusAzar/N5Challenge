using Microsoft.EntityFrameworkCore;

namespace N5Backend.Data;

public class AppDbContext : DbContext
{
    public DbSet<PermissionType> PermissionTypes { get; set; }
    public DbSet<Permission> Permissions { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Permission>()
            .HasOne<PermissionType>(p => p.TipoPermiso)
            .WithMany()
            .HasForeignKey(p => p.TipoPermisoId);
    }
}