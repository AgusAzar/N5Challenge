using N5Backend.Data;

namespace N5Backend.Services;

public interface IUnitOfWork : IDisposable
{
    public IRepository<Permission> Permissions { get; }
    public IRepository<PermissionType> PermissionTypes { get; }
    Task<int> CompleteAsync();
}

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    public IRepository<Permission> Permissions { get; }
    public IRepository<PermissionType> PermissionTypes { get; }

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
        Permissions = new Repository<Permission>(context);
        PermissionTypes = new Repository<PermissionType>(context);
    }

    public async Task<int> CompleteAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}