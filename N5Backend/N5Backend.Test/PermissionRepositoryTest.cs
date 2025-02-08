using N5Backend.Data;
using N5Backend.Services;

namespace N5Backend.Test;

public class PermissionRepositoryTest
{
    private readonly AppDbContext _context;
    private readonly Repository<Permission> _permissionRepository;

    public PermissionRepositoryTest()
    {
        _context = FakeDbContext.GetDatabaseContext();
        _permissionRepository = new Repository<Permission>(_context);
    }
    
    [Fact]
    public async Task GetAllPermissions_ReturnsAllPermissions()
    {
        var result = await _permissionRepository.GetAllAsync();
        Assert.NotNull(result);
        Assert.Single(result);
    }

    [Fact]
    public async Task GetPermissionById_ReturnsPermission()
    {
        var result = await _permissionRepository.GetByIdAsync(1);
        Assert.NotNull(result);
        Assert.Equal("Agustin", result.NombreEmpleado);
        Assert.Equal("Azar", result.ApellidoEmpleado);
        Assert.Equal(1, result.TipoPermisoId);
    }

    [Fact]
    public async Task AddPermission_AddsPermission()
    {
        var permission = new Permission() { Id = 2, NombreEmpleado = "Jane", ApellidoEmpleado = "Smith", TipoPermisoId = 2 };
        
        await _permissionRepository.AddAsync(permission);
        await _context.SaveChangesAsync();

        var result = await _permissionRepository.GetByIdAsync(2);

        Assert.NotNull(result);
        Assert.Equal("Jane", result.NombreEmpleado);
        Assert.Equal("Smith", result.ApellidoEmpleado);
        Assert.Equal(2, result.TipoPermisoId);

        var permissions = await _permissionRepository.GetAllAsync();
        Assert.Equal(2, permissions.Count());
    }

    [Fact]
    public async Task UpdatePermission_UpdatesPermission()
    {
        var permission = await _permissionRepository.GetByIdAsync(1);
        Assert.NotNull(permission);
        permission.NombreEmpleado = "Ezequiel";
        await _permissionRepository.UpdateAsync(permission);
        await _context.SaveChangesAsync();
        var result = await _permissionRepository.GetByIdAsync(1);
        Assert.NotNull(result);
        Assert.Equal("Ezequiel", result.NombreEmpleado);
    }
}