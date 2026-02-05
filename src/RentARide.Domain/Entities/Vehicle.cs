using RentARide.Domain.Enums;

namespace RentARide.Domain.Entities;

public class Vehicle : BaseEntity
{
    public string Model { get; set; } = string.Empty;
    public int Year { get; set; }
    public string LicensePlate { get; set; } = string.Empty;
    public decimal DailyPrice { get; set; }
    public VehicleStatus Status { get; set; }

    public int VehicleTypeId { get; set; }
    public VehicleType VehicleType { get; set; } = null!;

    public VehicleMaintenance? Maintenance { get; set; }
    public ICollection<Rental> Rentals { get; set; } = new List<Rental>();
}
