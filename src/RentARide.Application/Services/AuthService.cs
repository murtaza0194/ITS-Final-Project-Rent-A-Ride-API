using Microsoft.EntityFrameworkCore;
using RentARide.Application.Common.Interfaces;
using RentARide.Application.Common.Models;
using RentARide.Domain.Entities;
using RentARide.Domain.Enums;
using BCrypt.Net;

namespace RentARide.Application.Services;

public class AuthService : IAuthService
{
    private readonly IApplicationDbContext _context;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public AuthService(IApplicationDbContext context, IJwtTokenGenerator jwtTokenGenerator)
    {
        _context = context;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<ServiceResult<string>> LoginAsync(string email, string password)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (user == null)
            return ServiceResult<string>.Failure("Invalid credentials", 401);

        if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
             return ServiceResult<string>.Failure("Invalid credentials", 401);

        var token = _jwtTokenGenerator.GenerateToken(user);
        
        return ServiceResult<string>.Ok(token, "Login successful");
    }

    public async Task<ServiceResult<string>> RegisterAsync(string firstName, string lastName, string email, string password, UserRole role)
    {
        if (await _context.Users.AnyAsync(u => u.Email == email))
            return ServiceResult<string>.Failure("Email already exists", 400);

        var user = new User
        {
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
            Role = role
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync(CancellationToken.None);

        return ServiceResult<string>.Ok(user.Id.ToString(), "User registered successfully", 201);
    }
}
