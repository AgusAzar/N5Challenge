using Microsoft.AspNetCore.Mvc;
using N5Backend.Services;

namespace N5Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PermissionTypesController: ControllerBase
{
    private readonly IPermissionTypeService _permissionTypeService;

    public PermissionTypesController(IPermissionTypeService permissionTypeService)
    {
        _permissionTypeService = permissionTypeService;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return Ok(await _permissionTypeService.GetPermissionTypesAsync());
    }
}