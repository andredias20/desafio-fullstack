using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WeatherApp.Domain.Entities;

namespace WeatherApp.Infrastructure.Persistence.Configurations;

public class TemperatureRecordConfiguration : IEntityTypeConfiguration<TemperatureRecord>
{
    public void Configure(EntityTypeBuilder<TemperatureRecord> builder)
    {
        builder
            .ToTable("temperature_records")
            .HasKey(r => r.Id)
            .HasName("pk_temperature_record");

        builder
            .Property(r => r.CityName)
            .HasMaxLength(100);
        
        builder
            .Property(r => r.RecordedAt)
            .HasColumnType("timestamp with time zone");

        builder
            .HasIndex(r => r.RecordedAt)
            .HasDatabaseName("ix_temperature_records_recorded_at");
    }
}