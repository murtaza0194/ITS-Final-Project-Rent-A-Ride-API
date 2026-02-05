using Microsoft.AspNetCore.Mvc;
using RentARide.Application.Common.Interfaces;
using RentARide.Domain.Entities;
using RentARide.Application.Common.Models;

namespace RentARide.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class RentalsController : ControllerBase
{
    private readonly IRentalService _rentalService;

    public RentalsController(IRentalService rentalService)
    {
        _rentalService = rentalService;
    }

    [HttpPost]
    public async Task<ActionResult<ServiceResult<int>>> Book(BookRentalRequest request)
    {
        var result = await _rentalService.BookRentalAsync(request.UserId, request.VehicleId, request.StartDate, request.EndDate, request.AmenityIds);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("my-history")]
    public async Task<ActionResult<ServiceResult<List<Rental>>>> MyHistory(int userId, int page = 1)
    {
        var result = await _rentalService.GetMyHistoryAsync(userId, page, 10);
        return StatusCode(result.StatusCode, result);
    }
}

public record BookRentalRequest(int UserId, int VehicleId, DateTime StartDate, DateTime EndDate, List<int> AmenityIds);
