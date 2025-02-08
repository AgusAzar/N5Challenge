using N5Backend.Data;
using N5Backend.Dtos;

namespace N5Backend.Services;

public interface IPermissionService
{
    Task<IEnumerable<Permission>> GetAllAsync();
    Task<Permission> CreateAsync(RequestPermissionDto requestPermissionDtop);
    Task<Permission> UpdateAsync(int id, RequestPermissionDto requestPermissionDtop);
}

public class PermissionService : IPermissionService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IElasticService _elasticService;


    public PermissionService(IUnitOfWork unitOfWork, IElasticService elasticService)
    {
        _unitOfWork = unitOfWork;
        _elasticService = elasticService;
    }

    public Task<IEnumerable<Permission>> GetAllAsync()
    {
        return _unitOfWork.Permissions.GetAllAsync();
    }

    public async Task<Permission> CreateAsync(RequestPermissionDto requestPermissionDtop)
    {
        var tipoPermiso = await _unitOfWork.PermissionTypes.GetByIdAsync(requestPermissionDtop.TipoPermisoId);
        if (tipoPermiso == null)
        {
            throw new ArgumentException("Tipo permiso no valido");
        }

        var permission = new Permission()
        {
            NombreEmpleado = requestPermissionDtop.NombreEmpleado,
            ApellidoEmpleado = requestPermissionDtop.ApellidoEmpleado,
            TipoPermisoId = requestPermissionDtop.TipoPermisoId,
            TipoPermiso = tipoPermiso
        };
        await _unitOfWork.Permissions.AddAsync(permission);
        await _unitOfWork.CompleteAsync();

        var indexResponse = await _elasticService.AddAsync(permission);
        if (!indexResponse.IsValidResponse)
        {
            throw new Exception("Error al indexar el permiso");
        }

        return permission;
    }

    public async Task<Permission> UpdateAsync(int id, RequestPermissionDto requestPermissionDtop)
    {
        var permission = await _unitOfWork.Permissions.GetByIdAsync(id);
        if (permission is null)
        {
            throw new KeyNotFoundException("permiso no encontrado");
        }

        permission.NombreEmpleado = requestPermissionDtop.NombreEmpleado;
        permission.ApellidoEmpleado = requestPermissionDtop.ApellidoEmpleado;
        permission.TipoPermisoId = requestPermissionDtop.TipoPermisoId;

        await _unitOfWork.Permissions.UpdateAsync(permission);
        await _unitOfWork.CompleteAsync();

        var indexResponse = await _elasticService.AddOrUpdateAsync(permission);
        if (!indexResponse.IsValidResponse)
        {
            throw new Exception("Error al indexar el permiso");
        }

        return permission;
    }
}