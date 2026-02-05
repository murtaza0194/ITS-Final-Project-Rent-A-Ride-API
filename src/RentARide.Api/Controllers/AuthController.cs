using Microsoft.AspNetCore.Mvc;
using RentARide.Application.Common.Interfaces;
using RentARide.Domain.Enums;
using RentARide.Application.Common.Models;

namespace RentARide.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<ServiceResult<string>>> Register(RegisterRequest request)
    {
        var result = await _authService.RegisterAsync(request.FirstName, request.LastName, request.Email, request.Password, UserRole.Customer);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("login")]
    public async Task<ActionResult<ServiceResult<string>>> Login(LoginRequest request)
    {
        var result = await _authService.LoginAsync(request.Email, request.Password);
        return StatusCode(result.StatusCode, result);
    }
}

public record RegisterRequest(string FirstName, string LastName, string Email, string Password);
public record LoginRequest(string Email, string Password);
