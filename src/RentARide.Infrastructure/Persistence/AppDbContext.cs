using Microsoft.EntityFrameworkCore;
using RentARide.Domain.Entities;
using RentARide.Infrastructure.Interceptors;
using RentARide.Application.Common.Interfaces;
using System.Reflection;

namespace RentARide.Infrastructure.Persistence;

public class AppDbContext : DbContext, IApplicationDbContext
{
    private readonly AuditLogInterceptor _auditLogInterceptor;

    public AppDbContext(DbContextOptions<AppDbContext> options, AuditLogInterceptor auditLogInterceptor) : base(options)
    {
        _auditLogInterceptor = auditLogInterceptor;
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Vehicle> Vehicles { get; set; }
    public DbSet<VehicleType> VehicleTypes { get; set; }
    public DbSet<VehicleMaintenance> VehicleMaintenances { get; set; }
    public DbSet<Rental> Rentals { get; set; }
    public DbSet<Amenity> Amenities { get; set; }
    public DbSet<RentalAmenity> RentalAmenities { get; set; }
    public DbSet<AuditLog> AuditLogs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(_auditLogInterceptor);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}
