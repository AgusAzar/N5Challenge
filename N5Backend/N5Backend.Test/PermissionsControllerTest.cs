using Microsoft.AspNetCore.Mvc;
using Moq;
using N5Backend.Controllers;
using N5Backend.Data;
using N5Backend.Dtos;
using N5Backend.Services;

namespace N5Backend.Test;

public class PermissionsControllerTest
{
    private readonly Mock<IPermissionService> _mockPermissionService;
    private readonly PermissionsController _controller;

    public PermissionsControllerTest()
    {
        _mockPermissionService = new Mock<IPermissionService>();
        _controller = new PermissionsController(_mockPermissionService.Object);
    }

    [Fact]
    public async Task RequestPermission_ReturnsOkResult_WhenPermissionIsCreated()
    {
        var permissionType = new PermissionType { Id = 1, Descripcion = "Vacaciones" };
        var requestPermissionDto = new RequestPermissionDto()
        {
            NombreEmpleado = "Agustin",
            ApellidoEmpleado = "Azar",
            TipoPermisoId = permissionType.Id,
        };

        var expectedPermission = new Permission()
        {
            NombreEmpleado = requestPermissionDto.NombreEmpleado,
            ApellidoEmpleado = requestPermissionDto.ApellidoEmpleado,
            TipoPermisoId = requestPermissionDto.TipoPermisoId,
            TipoPermiso = permissionType
        };

        _mockPermissionService.Setup(service => service.CreateAsync(requestPermissionDto))
            .ReturnsAsync(expectedPermission);

        var result = await _controller.RequestPermission(requestPermissionDto);
        var okResult = Assert.IsType<OkObjectResult>(result);

        Assert.Equal(expectedPermission, okResult.Value);
    }

    [Fact]
    public async Task RequestPermission_ReturnsBadRequest_WhenPermissionTypeDoesNotExist()
    {
        _mockPermissionService.Setup(service => service.CreateAsync(It.IsAny<RequestPermissionDto>()))
            .Throws(new ArgumentException("Tipo de permiso invalido"));
        var result = await _controller.RequestPermission(new RequestPermissionDto());
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task GetPermissions_ReturnsOkResult_WithPermissionsList()
    {
        var expectedPermissions = new List<Permission>
        {
            new Permission
            {
                Id = 1, NombreEmpleado = "Agustin", ApellidoEmpleado = "Azar", TipoPermisoId = 1,
                FechaPermiso = DateTime.UtcNow
            },
            new Permission
            {
                Id = 2, NombreEmpleado = "Pepe", ApellidoEmpleado = "Argento", TipoPermisoId = 2,
                FechaPermiso = DateTime.UtcNow
            }
        };

        _mockPermissionService.Setup(service => service.GetAllAsync())
            .ReturnsAsync(expectedPermissions);

        var result = await _controller.GetPermissions();
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(expectedPermissions, okResult.Value);
    }

    [Fact]
    public async Task ModifyPermission_ReturnsOkResult_WhenPermissionIsModified()
    {
        var originalPermission = new Permission
        {
            Id = 1, NombreEmpleado = "Agustin", ApellidoEmpleado = "Azar", TipoPermisoId = 1,
            FechaPermiso = DateTime.UtcNow, TipoPermiso = new PermissionType()
            {
                Id = 1,
                Descripcion = "Test"
            }
        };
        var permissionDto = new RequestPermissionDto()
        {
            NombreEmpleado = "Pepe",
            ApellidoEmpleado = "Argento",
            TipoPermisoId = 2,
        };
        var expectedPermission = new Permission()
        {
            Id = 1, NombreEmpleado = "Pepe", ApellidoEmpleado = "Pepe", TipoPermisoId = 2,
            FechaPermiso = DateTime.UtcNow, TipoPermiso = new PermissionType()
            {
                Id = 2,
                Descripcion = "test 2"
            }
        };
        _mockPermissionService.Setup(service => service.UpdateAsync(originalPermission.Id, permissionDto))
            .ReturnsAsync(expectedPermission);
        var result = await _controller.ModifyPermission(originalPermission.Id, permissionDto);
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(expectedPermission, okResult.Value);
    }

    [Fact]
    public async Task ModifyPermission_ReturnsNotFound_WhenPermissionDoesNotExist()
    {
        var resultMessage = "Permiso no encontrado";
        _mockPermissionService.Setup(service => service.UpdateAsync(It.IsAny<int>(), It.IsAny<RequestPermissionDto>()))
            .Throws(new KeyNotFoundException(resultMessage));
        
        var result = await _controller.ModifyPermission(1, new RequestPermissionDto());
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(resultMessage, notFoundResult.Value);
    }
}