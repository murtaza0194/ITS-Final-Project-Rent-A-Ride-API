using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using RentARide.Application.Common.Interfaces;
using RentARide.Application.Services;
using System.Reflection;

namespace RentARide.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        
        // Mapster
        var config = Mapster.TypeAdapterConfig.GlobalSettings;
        config.Scan(Assembly.GetExecutingAssembly());
        services.AddSingleton(config);
        services.AddScoped<MapsterMapper.IMapper, MapsterMapper.ServiceMapper>();
        
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IVehicleService, VehicleService>();
        services.AddScoped<IRentalService, RentalService>();

        return services;
    }
}
