using RentARide.Domain.Entities;
using RentARide.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using RentARide.Application.Common.Interfaces;

namespace RentARide.Infrastructure.Persistence;

public static class DbInitializer
{
    public static async Task SeedAsync(IApplicationDbContext context)
    {
        // Ensure Database Created
        // In production, use migrations. For this demo, EnsureCreated is fine for SQLite.
        // But Program.cs likely calls Migrate. We'll just add data.
        
        if (!await context.VehicleTypes.AnyAsync())
        {
            var types = new List<VehicleType>
            {
                new VehicleType { Name = "Economy", Description = "Fuel-efficient compact cars" },
                new VehicleType { Name = "SUV", Description = "Spacious utility vehicles for families" },
                new VehicleType { Name = "Luxury", Description = "Premium driving experience" },
                new VehicleType { Name = "Sports", Description = "High performance cars" }
            };
            
            context.VehicleTypes.AddRange(types);
            await context.SaveChangesAsync(CancellationToken.None);

            // Fetch IDs after save
            var economy = types.First(t => t.Name == "Economy");
            var suv = types.First(t => t.Name == "SUV");
            var luxury = types.First(t => t.Name == "Luxury");

            if (!await context.Vehicles.AnyAsync())
            {
                context.Vehicles.AddRange(new List<Vehicle>
                {
                    new Vehicle { 
                        Model = "Toyota Corolla", 
                        Year = 2024, 
                        LicensePlate = "IQ-12345", 
                        DailyPrice = 40, 
                        Status = VehicleStatus.Available, 
                        VehicleTypeId = economy.Id,
                        VehicleType = economy
                    },
                     new Vehicle { 
                        Model = "Hyundai Tucson", 
                        Year = 2023, 
                        LicensePlate = "IQ-55555", 
                        DailyPrice = 70, 
                        Status = VehicleStatus.Available, 
                        VehicleTypeId = suv.Id,
                         VehicleType = suv
                    },
                     new Vehicle { 
                        Model = "Mercedes C-Class", 
                        Year = 2025, 
                        LicensePlate = "IQ-99999", 
                        DailyPrice = 150, 
                        Status = VehicleStatus.Available, 
                        VehicleTypeId = luxury.Id,
                         VehicleType = luxury
                    },
                    new Vehicle { 
                        Model = "Kia Picanto", 
                        Year = 2022, 
                        LicensePlate = "IQ-11111", 
                        DailyPrice = 30, 
                        Status = VehicleStatus.Available, 
                        VehicleTypeId = economy.Id,
                         VehicleType = economy
                    }
                });
                
                await context.SaveChangesAsync(CancellationToken.None);
            }

            // Ensure Sports Cars are added even if DB exists
            var sports = types.First(t => t.Name == "Sports");
            if (!await context.Vehicles.AnyAsync(v => v.Model == "Ferrari 488 Spider"))
            {
                 context.Vehicles.AddRange(new List<Vehicle>
                {
                    new Vehicle { Model = "Ferrari 488 Spider", Year = 2024, LicensePlate = "IQ-SPORTS-1", DailyPrice = 500, Status = VehicleStatus.Available, VehicleTypeId = sports.Id, VehicleType = sports },
                    new Vehicle { Model = "Lamborghini Huracan", Year = 2024, LicensePlate = "IQ-SPORTS-2", DailyPrice = 600, Status = VehicleStatus.Available, VehicleTypeId = sports.Id, VehicleType = sports },
                    new Vehicle { Model = "Porsche 911 Carrera", Year = 2025, LicensePlate = "IQ-SPORTS-3", DailyPrice = 400, Status = VehicleStatus.Available, VehicleTypeId = sports.Id, VehicleType = sports },
                    new Vehicle { Model = "BMW M4", Year = 2023, LicensePlate = "IQ-SPORTS-4", DailyPrice = 350, Status = VehicleStatus.Available, VehicleTypeId = sports.Id, VehicleType = sports }
                });
                await context.SaveChangesAsync(CancellationToken.None);
            }
        }
    }
}
