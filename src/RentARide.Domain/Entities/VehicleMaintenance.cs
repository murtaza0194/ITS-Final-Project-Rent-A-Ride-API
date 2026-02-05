namespace RentARide.Domain.Entities;

public class VehicleMaintenance : BaseEntity
{
    public string Description { get; set; } = string.Empty;
    public DateTime LastMaintenanceDate { get; set; }
    public DateTime NextMaintenanceDue { get; set; }

    public int VehicleId { get; set; }
    public Vehicle Vehicle { get; set; } = null!;
}
