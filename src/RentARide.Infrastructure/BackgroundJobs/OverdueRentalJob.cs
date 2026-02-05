using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RentARide.Application.Common.Interfaces;
using RentARide.Domain.Enums;

namespace RentARide.Infrastructure.BackgroundJobs;

public class OverdueRentalJob
{
    private readonly IApplicationDbContext _context;
    private readonly ILogger<OverdueRentalJob> _logger;

    public OverdueRentalJob(IApplicationDbContext context, ILogger<OverdueRentalJob> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task Process()
    {
        var overdueRentals = await _context.Rentals
            .Where(r => r.Status == RentalStatus.Active && r.EndDate < DateTime.UtcNow)
            .AsNoTracking()
            .ToListAsync();

        foreach (var rental in overdueRentals)
        {
            _logger.LogWarning("Rental {Id} is overdue. User {UserId} has not returned the car.", rental.Id, rental.UserId);
        }
    }
}
