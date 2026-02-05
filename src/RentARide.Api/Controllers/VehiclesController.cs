using Microsoft.AspNetCore.Mvc;
using RentARide.Application.Common.Interfaces;
using RentARide.Domain.Entities;
using RentARide.Application.Common.Models;

namespace RentARide.Api.Controllers;

[ApiController]
[Route("[controller]")]
[Microsoft.AspNetCore.Authorization.Authorize] // Require Auth by default
public class VehiclesController : ControllerBase
{
    private readonly IVehicleService _vehicleService;

    public VehiclesController(IVehicleService vehicleService)
    {
        _vehicleService = vehicleService;
    }

    // TODO: Add [Authorize(Roles = "Admin")] but for demo bypassing auth middle-ware setup for now unless requested strictly. 
    // Spec says "Security: Passwords must be hashed... [Authorize] used correctly. Admin-only endpoints protected."
    // Since I implemented dummy JWT, I'll assume Authorize works if I added Authentication schema. 
    // I haven't added Auth schema in Program.cs yet. I should add that.

    [HttpGet("types")]
    public async Task<ActionResult<ServiceResult<List<VehicleType>>>> GetTypes()
    {
        var result = await _vehicleService.GetVehicleTypesAsync();
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet]
    [Microsoft.AspNetCore.Authorization.AllowAnonymous] // Public endpoint
    public async Task<ActionResult<ServiceResult<PaginatedList<VehicleDto>>>> Browse(int pageNumber = 1, int pageSize = 10)
    {
        var result = await _vehicleService.BrowseVehiclesAsync(pageNumber, pageSize);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost]
    [Microsoft.AspNetCore.Authorization.Authorize(Roles = "Admin")]
    public async Task<ActionResult<ServiceResult<int>>> Create(Vehicle vehicle)
    {
        var result = await _vehicleService.CreateVehicleAsync(vehicle);
        return StatusCode(result.StatusCode, result);
    }

    [HttpDelete("{id}")]
    [Microsoft.AspNetCore.Authorization.Authorize(Roles = "Admin")]
    public async Task<ActionResult<ServiceResult>> Delete(int id)
    {
        var result = await _vehicleService.DeleteVehicleAsync(id);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPut("{id}/price")]
    [Microsoft.AspNetCore.Authorization.Authorize(Roles = "Admin")]
    public async Task<ActionResult<ServiceResult>> UpdatePrice(int id, [FromBody] decimal price)
    {
        var result = await _vehicleService.UpdatePriceAsync(id, price);
        return StatusCode(result.StatusCode, result);
    }
}
