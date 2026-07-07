using Backend_DispenXCore.Api.src.Dispensadores.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend_DispenXCore.Api.src.Dispensadores.Infrastructure.Persistence.Configurations;

public class DispenserEventConfiguration : IEntityTypeConfiguration<DispenserEvent>
{
    public void Configure(EntityTypeBuilder<DispenserEvent> builder)
    {
        builder.ToTable("DispenserEvents");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.AmountDispensed).HasColumnType("decimal(18,2)");
    }
}
