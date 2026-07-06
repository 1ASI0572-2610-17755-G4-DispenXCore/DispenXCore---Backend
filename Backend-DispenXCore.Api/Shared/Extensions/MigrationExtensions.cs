using Microsoft.EntityFrameworkCore;
using Backend_DispenXCore.Api.Infrastructure.Persistence;

namespace Backend_DispenXCore.Api.Shared.Extensions;

public static class MigrationExtensions
{
    public static void ApplyMigrations(this WebApplication app)
    {
        // Se crea un scope para obtener servicios registrados en el contenedor de dependencias.
        using var scope = app.Services.CreateScope();
        // Se obtiene el contexto de base de datos de la aplicación.
        var db = scope.ServiceProvider.GetRequiredService<DispenXDbContext>();
        // Aplica automáticamente las migraciones pendientes a la base de datos.
        db.Database.Migrate();
    }
}
