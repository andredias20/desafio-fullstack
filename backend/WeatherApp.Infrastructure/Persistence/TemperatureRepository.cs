using Microsoft.EntityFrameworkCore;
using WeatherApp.Domain.Entities;
using WeatherApp.Domain.Interfaces;

namespace WeatherApp.Infrastructure.Persistence;

public class TemperatureRepository : ITemperatureRepository
{
    private readonly AppDbContext _context;
    
    public TemperatureRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(TemperatureRecord record, CancellationToken ct)
    {
        await _context.AddAsync(record, ct);
        await _context.SaveChangesAsync(ct);
    }

    public async Task<IEnumerable<TemperatureRecord>> GetHistoryAsync(string? cityName, double? latitude, double? longitude, CancellationToken ct)
    {
        var query = _context.Set<TemperatureRecord>()
            .Where(r => r.RecordedAt >= DateTime.UtcNow.AddDays(-30));

        if (cityName is not null)
            query = query.Where(r => r.CityName == cityName);
        else if (latitude is not null && longitude is not null)
            query = query.Where(r => r.Latitude == latitude && r.Longitude == longitude);

        return await query
            .OrderByDescending(r => r.RecordedAt)
            .ToListAsync(ct);
    }
}