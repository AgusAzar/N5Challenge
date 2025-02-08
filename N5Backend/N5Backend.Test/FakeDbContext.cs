using Microsoft.EntityFrameworkCore;
using N5Backend.Data;
namespace N5Backend.Test;

public static class FakeDbContext
{
    public static AppDbContext GetDatabaseContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Base de datos en memoria
            .Options;

        var dbContext = new AppDbContext(options);
        
        dbContext.PermissionTypes.Add(new PermissionType() { Id = 1, Descripcion = "Admin" });
        dbContext.PermissionTypes.Add(new PermissionType { Id = 2, Descripcion = "User" });
        
        dbContext.Permissions.Add(new Permission { Id = 1, NombreEmpleado = "Agustin", ApellidoEmpleado = "Azar", TipoPermisoId = 1 });
        dbContext.SaveChanges();

        return dbContext;
    }
}
