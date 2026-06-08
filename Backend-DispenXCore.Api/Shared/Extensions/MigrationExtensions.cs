using Microsoft.EntityFrameworkCore;
using Backend_DispenXCore.Api.Infrastructure.Persistence;

namespace Backend_DispenXCore.Api.Shared.Extensions;
public static class MigrationExtensions
{
    public static void ApplyMigrations(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<DispenXDbContext>();
        db.Database.Migrate();
    }
}