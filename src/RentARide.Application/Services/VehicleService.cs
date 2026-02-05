using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using RentARide.Application.Common.Interfaces;
using RentARide.Application.Common.Models;
using RentARide.Domain.Entities;

namespace RentARide.Application.Services;

public class VehicleService : IVehicleService
{
    private readonly IApplicationDbContext _context;
    private readonly IMemoryCache _cache;
    private const string VehicleTypesCacheKey = "vehicle_types";

    public VehicleService(IApplicationDbContext context, IMemoryCache cache)
    {
        _context = context;
        _cache = cache;
    }

    public async Task<ServiceResult<PaginatedList<VehicleDto>>> BrowseVehiclesAsync(int pageNumber, int pageSize)
    {
        var query = _context.Vehicles
            .AsNoTracking()
            .ProjectToType<VehicleDto>();

        var list = await PaginatedList<VehicleDto>.CreateAsync(query, pageNumber, pageSize);

        return ServiceResult<PaginatedList<VehicleDto>>.Ok(list);
    }

    public async Task<ServiceResult<int>> CreateVehicleAsync(Vehicle vehicle)
    {
        // vehicle is already validated by FluentValidation in Controller before calling this, or assume valid
        _context.Vehicles.Add(vehicle);
        await _context.SaveChangesAsync(CancellationToken.None);
        return ServiceResult<int>.Ok(vehicle.Id, "Vehicle created", 201);
    }

    public async Task<ServiceResult> DeleteVehicleAsync(int id)
    {
        var vehicle = await _context.Vehicles.FindAsync(id);
        if (vehicle == null) return ServiceResult.Failure("Vehicle not found", 404);

        // Soft delete handled by Interceptor when State is Deleted
        _context.Vehicles.Remove(vehicle);
        await _context.SaveChangesAsync(CancellationToken.None);
        
        return ServiceResult.Ok("Vehicle deleted");
    }

    public async Task<ServiceResult<List<VehicleType>>> GetVehicleTypesAsync()
    {
        return await _cache.GetOrCreateAsync(VehicleTypesCacheKey, async entry =>
        {
            entry.SlidingExpiration = TimeSpan.FromHours(1);
            return ServiceResult<List<VehicleType>>.Ok(await _context.VehicleTypes.AsNoTracking().ToListAsync());
        }) ?? ServiceResult<List<VehicleType>>.Failure("Failed to retrieve types");
    }

    public async Task<ServiceResult> UpdatePriceAsync(int id, decimal newPrice)
    {
        var vehicle = await _context.Vehicles.FindAsync(id);
        if (vehicle == null) return ServiceResult.Failure("Vehicle not found", 404);

        vehicle.DailyPrice = newPrice;
        await _context.SaveChangesAsync(CancellationToken.None);

        return ServiceResult.Ok("Price updated");
    }
}
