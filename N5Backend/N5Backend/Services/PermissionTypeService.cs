using N5Backend.Data;

namespace N5Backend.Services;

public interface IPermissionTypeService
{ 
    Task<IEnumerable<PermissionType>> GetPermissionTypesAsync();
}

public class PermissionTypeService: IPermissionTypeService
{
    private readonly IUnitOfWork _unitOfWork;

    public PermissionTypeService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public Task<IEnumerable<PermissionType>> GetPermissionTypesAsync()
    {
        return _unitOfWork.PermissionTypes.GetAllAsync();
    }
}