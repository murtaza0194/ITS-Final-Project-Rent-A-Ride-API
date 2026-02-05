using RentARide.Application;
using RentARide.Infrastructure;
using RentARide.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Hangfire;
using RentARide.Api.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        policy =>
        {
            policy.WithOrigins("http://localhost:5173")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

builder.Services.AddAuthentication()
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JwtSettings:Issuer"] ?? "RentARide",
            ValidAudience = builder.Configuration["JwtSettings:Audience"] ?? "RentARideUsers",
            IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(
                System.Text.Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Secret"] ?? "super-secret-key-that-is-long-enough-for-hs256"))
        };
    });
builder.Services.AddAuthorization();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowReactApp");

app.UseAuthentication();
app.UseAuthorization();

app.UseExceptionHandler();

app.UseHangfireDashboard();

// Register Recurring Job
RecurringJob.AddOrUpdate<RentARide.Infrastructure.BackgroundJobs.OverdueRentalJob>("overdue-rentals", job => job.Process(), Cron.Hourly);

app.MapControllers();

// Ensure DB is created & Seed Data
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<RentARide.Infrastructure.Persistence.AppDbContext>();
    // context.Database.EnsureCreated(); // Or Migrate
    context.Database.Migrate(); 
    
    // Seed
    await RentARide.Infrastructure.Persistence.DbInitializer.SeedAsync(context);
}

app.Run();
