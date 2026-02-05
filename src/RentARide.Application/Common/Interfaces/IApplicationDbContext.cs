using Microsoft.EntityFrameworkCore;
using RentARide.Domain.Entities;

namespace RentARide.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; }
    DbSet<Vehicle> Vehicles { get; }
    DbSet<VehicleType> VehicleTypes { get; }
    DbSet<VehicleMaintenance> VehicleMaintenances { get; }
    DbSet<Rental> Rentals { get; }
    DbSet<Amenity> Amenities { get; }
    DbSet<RentalAmenity> RentalAmenities { get; }
    DbSet<AuditLog> AuditLogs { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
