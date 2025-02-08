using Elastic.Clients.Elasticsearch;
using Microsoft.AspNetCore.Mvc;
using N5Backend.Dtos;
using N5Backend.Services;

namespace N5Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PermissionsController : ControllerBase
{
    private readonly IPermissionService _permissionService;
    
    public PermissionsController(IPermissionService permissionService)
    {
        _permissionService = permissionService;
    }

    [HttpGet]
    public async Task<IActionResult> GetPermissions()
    {
        return Ok(await _permissionService.GetAllAsync());
    }

    [HttpPost]
    public async Task<IActionResult> RequestPermission([FromBody] RequestPermissionDto permissionRequestDtop)
    {
        try
        {
            var permission = await _permissionService.CreateAsync(permissionRequestDtop);
            return Ok(permission);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> ModifyPermission(int id, [FromBody] RequestPermissionDto permissionRequestDtop)
    {
        try
        {
            return Ok(await _permissionService.UpdateAsync(id, permissionRequestDtop));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}