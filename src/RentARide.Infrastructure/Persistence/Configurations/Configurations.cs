using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RentARide.Domain.Entities;

namespace RentARide.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasIndex(u => u.Email).IsUnique();
        builder.Property(u => u.Email).IsRequired().HasMaxLength(200);
        builder.Property(u => u.FirstName).IsRequired().HasMaxLength(100);
        builder.Property(u => u.LastName).IsRequired().HasMaxLength(100);
        
        builder.HasMany(u => u.Rentals)
            .WithOne(r => r.User)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class VehicleConfiguration : IEntityTypeConfiguration<Vehicle>
{
    public void Configure(EntityTypeBuilder<Vehicle> builder)
    {
        builder.Property(v => v.Model).IsRequired().HasMaxLength(100);
        builder.Property(v => v.LicensePlate).IsRequired().HasMaxLength(20);
        
        builder.HasOne(v => v.VehicleType)
            .WithMany(vt => vt.Vehicles)
            .HasForeignKey(v => v.VehicleTypeId);

        builder.HasOne(v => v.Maintenance)
            .WithOne(vm => vm.Vehicle)
            .HasForeignKey<VehicleMaintenance>(vm => vm.VehicleId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class RentalConfiguration : IEntityTypeConfiguration<Rental>
{
    public void Configure(EntityTypeBuilder<Rental> builder)
    {
        builder.Property(r => r.TotalPrice).HasColumnType("decimal(18,2)");
        
        builder.HasOne(r => r.Vehicle)
            .WithMany(v => v.Rentals)
            .HasForeignKey(r => r.VehicleId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class RentalAmenityConfiguration : IEntityTypeConfiguration<RentalAmenity>
{
    public void Configure(EntityTypeBuilder<RentalAmenity> builder)
    {
         builder.HasKey(ra => new { ra.RentalId, ra.AmenityId });

         builder.HasOne(ra => ra.Rental)
             .WithMany(r => r.RentalAmenities)
             .HasForeignKey(ra => ra.RentalId);
         
         builder.HasOne(ra => ra.Amenity)
             .WithMany(a => a.RentalAmenities)
             .HasForeignKey(ra => ra.AmenityId);
    }
}

public class AmenityConfiguration : IEntityTypeConfiguration<Amenity>
{
    public void Configure(EntityTypeBuilder<Amenity> builder)
    {
        builder.Property(a => a.Price).HasColumnType("decimal(18,2)");
        builder.Property(a => a.Name).IsRequired().HasMaxLength(100);
    }
}

public class VehicleTypeConfiguration : IEntityTypeConfiguration<VehicleType>
{
    public void Configure(EntityTypeBuilder<VehicleType> builder)
    {
        builder.Property(vt => vt.Name).IsRequired().HasMaxLength(50);
    }
}
