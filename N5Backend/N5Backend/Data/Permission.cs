namespace N5Backend.Data;

public class Permission
{
    public int Id { get; set; }
    public required string NombreEmpleado { get; set; }
    public required string ApellidoEmpleado { get; set; }
    public int TipoPermisoId { get; set; }
    public virtual PermissionType TipoPermiso { get; set; }
    public DateTime FechaPermiso { get; set; } = DateTime.Now;
    
}