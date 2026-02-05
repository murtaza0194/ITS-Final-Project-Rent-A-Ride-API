using RentARide.Domain.Entities;

namespace RentARide.Application.Common.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user);
}
