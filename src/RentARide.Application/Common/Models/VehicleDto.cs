namespace RentARide.Application.Common.Models;

public class VehicleDto
{
    public int Id { get; set; }
    public string Model { get; set; } = string.Empty;
    public int Year { get; set; }
    public string LicensePlate { get; set; } = string.Empty;
    public decimal DailyPrice { get; set; }
    public string Status { get; set; } = string.Empty;
    public string VehicleType { get; set; } = string.Empty; // Mapped from VehicleType.Name
}
