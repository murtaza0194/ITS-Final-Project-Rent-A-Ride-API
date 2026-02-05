using RentARide.Application.Common.Models;
using RentARide.Domain.Entities;
using RentARide.Domain.Enums;

namespace RentARide.Application.Common.Interfaces;

public interface IAuthService
{
    Task<ServiceResult<string>> RegisterAsync(string firstName, string lastName, string email, string password, UserRole role);
    Task<ServiceResult<string>> LoginAsync(string email, string password);
}

public interface IVehicleService
{
    Task<ServiceResult<int>> CreateVehicleAsync(Vehicle vehicle);
    Task<ServiceResult> UpdatePriceAsync(int id, decimal newPrice);
    Task<ServiceResult> DeleteVehicleAsync(int id);
    Task<ServiceResult<List<VehicleType>>> GetVehicleTypesAsync(); // Cached
    Task<ServiceResult<PaginatedList<VehicleDto>>> BrowseVehiclesAsync(int pageNumber, int pageSize);
}

public interface IRentalService
{
    Task<ServiceResult<int>> BookRentalAsync(int userId, int vehicleId, DateTime startDate, DateTime endDate, List<int> amenityIds);
    Task<ServiceResult<List<Rental>>> GetMyHistoryAsync(int userId, int pageNumber, int pageSize);
}
