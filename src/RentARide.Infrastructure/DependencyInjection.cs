using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RentARide.Application.Common.Interfaces;
using RentARide.Infrastructure.Interceptors;
using RentARide.Infrastructure.Persistence;
using RentARide.Infrastructure.Services;
using Hangfire;
using Hangfire.MemoryStorage;

namespace RentARide.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<AuditLogInterceptor>();

        // Use InMemory DB for ease of setup/demo as requested "Init your solution... setup Postgres OR ANY OTHER DB"
        // User Plan accepted SQLite.
        services.AddDbContext<AppDbContext>((sp, options) =>
        {
            var interceptor = sp.GetRequiredService<AuditLogInterceptor>();
            options.UseSqlite("Data Source=RentARide.db")
                   .AddInterceptors(interceptor);
        });

        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<AppDbContext>());
        
        // Hangfire
        services.AddHangfire(config => config
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseMemoryStorage()); // Using Memory Storage for demo/simplicity

        services.AddHangfireServer();

        // Services
        services.AddHttpClient("Nager.Date", client =>
        {
            client.BaseAddress = new Uri("https://date.nager.at/api/v3/");
        });
        
        services.AddTransient<IHolidayService, HolidayService>();
        services.AddTransient<RentARide.Infrastructure.BackgroundJobs.OverdueRentalJob>();
        services.AddSingleton<RentARide.Application.Common.Interfaces.IJwtTokenGenerator, RentARide.Infrastructure.Authentication.JwtTokenGenerator>();

        return services;
    }
}
