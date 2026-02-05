using Microsoft.EntityFrameworkCore;
using RentARide.Application.Common.Interfaces;
using RentARide.Application.Common.Models;
using RentARide.Domain.Entities;
using RentARide.Domain.Enums;

namespace RentARide.Application.Services;

public class RentalService : IRentalService
{
    private readonly IApplicationDbContext _context;
    private readonly IHolidayService _holidayService;

    public RentalService(IApplicationDbContext context, IHolidayService holidayService)
    {
        _context = context;
        _holidayService = holidayService;
    }

    public async Task<ServiceResult<int>> BookRentalAsync(int userId, int vehicleId, DateTime startDate, DateTime endDate, List<int> amenityIds)
    {
        // 1. Basic Validation
        if (endDate <= startDate)
            return ServiceResult<int>.Failure("EndDate must be greater than StartDate");

        // 2. Check Vehicle Availability
        // "Logic: Check if the car is already rented during those dates."
        // Overlap: existing.Start < requested.End && existing.End > requested.Start
        // Status must be Active (not Cancelled or Completed? Wait, Completed rentals don't block. Only Active. 
        // Or actually, even future booked/active rentals block. Status=Active usually means "Confirmed/Ongoing". 
        // I will assume Active rentals block dates. completed ones are past.)
        // Actually, if a rental is "Completed", it's done. But if it WAS scheduled for these dates, it keeps the history.
        // We only care about ensuring no overlap with "Active" rentals (which Includes future bookings).
        
        var isBooked = await _context.Rentals
            .AnyAsync(r => r.VehicleId == vehicleId && 
                           r.Status == RentalStatus.Active &&
                           r.StartDate < endDate && 
                           r.EndDate > startDate);

        if (isBooked)
            return ServiceResult<int>.Failure("Vehicle is already rented for these dates", 409); // Conflict

        var vehicle = await _context.Vehicles.FindAsync(vehicleId);
        if (vehicle == null) return ServiceResult<int>.Failure("Vehicle not found", 404);
        if (vehicle.Status != VehicleStatus.Available) return ServiceResult<int>.Failure("Vehicle is not available", 400);

        // 3. Calculate Price
        // Days * DailyPrice + Amenities
        var days = (endDate - startDate).Days;
        if (days == 0) days = 1; // Minimum 1 day calculation? Or fractional? Spec says "Days * DailyPrice". I'll assume Days.
        
        decimal totalPrice = days * vehicle.DailyPrice;

        var amenities = await _context.Amenities
            .Where(a => amenityIds.Contains(a.Id))
            .ToListAsync();
            
        foreach(var amenity in amenities)
        {
            totalPrice += amenity.Price;
        }

        // 4. Holiday Surcharge
        // "Logic: If the start date is a holiday in Germany country, add a 10% Surcharge to the total."
        if (await _holidayService.IsHolidayAsync(startDate, "DE"))
        {
            totalPrice += totalPrice * 0.10m;
        }

        // Create Entity
        var rental = new Rental
        {
            UserId = userId,
            VehicleId = vehicleId,
            StartDate = startDate,
            EndDate = endDate,
            TotalPrice = totalPrice,
            Status = RentalStatus.Active,
            RentalAmenities = amenities.Select(a => new RentalAmenity { AmenityId = a.Id }).ToList()
        };

        _context.Rentals.Add(rental);
        
        // Update vehicle status? 
        // Usually rental systems don't change vehicle status to "Rented" permanently, only effectively for the duration.
        // But spec says Vehicle.Status enum has "Rented". 
        // If I set it to Rented, it might block other future rentals. 
        // I'll leave Vehicle.Status as Available (meaning "In Service") and rely on Rental overlap check. 
        // Or if the rental starts *now*, I could set it. 
        // For simplicity, I will NOT change Vehicle.Status unless it's strictly required to be "Rented" right now.
        // Requirement 17: "Status (Available, Rented, Maintenance)". 
        // I'll stick to logic check for overlap.

        await _context.SaveChangesAsync(CancellationToken.None);

        return ServiceResult<int>.Ok(rental.Id, "Rental booked successfully", 201);
    }

    public async Task<ServiceResult<List<Rental>>> GetMyHistoryAsync(int userId, int pageNumber, int pageSize)
    {
         var list = await _context.Rentals
            .Where(r => r.UserId == userId)
            .Include(r => r.Vehicle)
            .OrderByDescending(r => r.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return ServiceResult<List<Rental>>.Ok(list);
    }
}
