namespace RentARide.Domain.Entities;

public class RentalAmenity : BaseEntity
{
    public int RentalId { get; set; }
    public Rental Rental { get; set; } = null!;

    public int AmenityId { get; set; }
    public Amenity Amenity { get; set; } = null!;
}
