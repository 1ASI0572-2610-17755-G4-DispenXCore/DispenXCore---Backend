using System.Text.Json;
using Backend_DispenXCore.Api.src.Dispensadores.Domain.Entities;
using Backend_DispenXCore.Api.src.Dispensadores.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend_DispenXCore.Api.src.Dispensadores.Infrastructure.Persistence.Configurations;

public class ScheduleConfiguration : IEntityTypeConfiguration<Schedule>
{
    public void Configure(EntityTypeBuilder<Schedule> builder)
    {
        builder.ToTable("schedules");
        builder.HasKey(s => s.Id);

        builder.Property(s => s.DispensatorId).IsRequired();
        builder.Property(s => s.Name).HasMaxLength(100);
        builder.Property(s => s.SupplyType)
            .HasConversion<string>()
            .HasMaxLength(20);
        builder.Property(s => s.Amount);
        builder.Property(s => s.ScheduledTime)
            .HasConversion(
                v => v.ToString(),
                v => TimeSpan.Parse(v))
            .HasMaxLength(8);

        // Conversión de FrequencyDays a JSON
        builder.Property(s => s.FrequencyDays)
            .HasConversion(
                v => JsonSerializer.Serialize(v.Days, (JsonSerializerOptions?)null),
                v => new FrequencyDays(JsonSerializer.Deserialize<List<int>>(v, (JsonSerializerOptions?)null) ?? new List<int>()))
            .HasColumnName("FrequencyDays");

        builder.Property(s => s.SmartRefill);
        builder.Property(s => s.IsActive);
    }
}