using System.Net.Http.Json;
using RentARide.Application.Common.Interfaces;

namespace RentARide.Infrastructure.Services;

public class HolidayService : IHolidayService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public HolidayService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<bool> IsHolidayAsync(DateTime date, string countryCode = "DE")
    {
        try
        {
            var client = _httpClientFactory.CreateClient("Nager.Date");
            var year = date.Year;
            var response = await client.GetAsync($"https://date.nager.at/api/v3/PublicHolidays/{year}/{countryCode}");

            if (!response.IsSuccessStatusCode)
            {
                // Log error or assume not a holiday in case of failure to prevent blocking? 
                // For now, return false if API fails.
                return false;
            }

            var holidays = await response.Content.ReadFromJsonAsync<List<PublicHolidayDto>>();
            
            if (holidays == null) return false;

            return holidays.Any(h => h.Date.Date == date.Date);
        }
        catch
        {
            // Fail safe
            return false;
        }
    }
}

public class PublicHolidayDto
{
    public DateTime Date { get; set; }
    public string LocalName { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string CountryCode { get; set; } = string.Empty;
}
