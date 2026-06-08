using Backend_DispenXCore.Api.src.IAM.Application.Interfaces;
using Backend_DispenXCore.Api.src.IAM.Application.UseCases;
using Backend_DispenXCore.Api.src.IAM.Domain.Services;
using Backend_DispenXCore.Api.src.IAM.Infrastructure.Persistence;
using Backend_DispenXCore.Api.src.IAM.Infrastructure.Security;
using Backend_DispenXCore.Api.src.Inventario.Application.Interfaces;
using Backend_DispenXCore.Api.src.Inventario.Application.UseCases;
using Backend_DispenXCore.Api.src.Inventario.Domain.Services;
using Backend_DispenXCore.Api.src.Inventario.Infrastructure.Persistence;
using Backend_DispenXCore.Api.src.AlertasStock.Application.Interfaces;
using Backend_DispenXCore.Api.src.AlertasStock.Application.UseCases;
using Backend_DispenXCore.Api.src.AlertasStock.Domain.Services;
using Backend_DispenXCore.Api.src.AlertasStock.Infrastructure.Persistence;
using Backend_DispenXCore.Api.src.AlertasStock.Infrastructure.Services;
using Backend_DispenXCore.Api.src.Usuarios.Application.Interfaces;
using Backend_DispenXCore.Api.src.Usuarios.Application.UseCases;
using Backend_DispenXCore.Api.src.Usuarios.Infrastructure.Persistence;
using Backend_DispenXCore.Api.src.Dispensadores.Application.Interfaces;
using Backend_DispenXCore.Api.src.Dispensadores.Application.UseCases;
using Backend_DispenXCore.Api.src.Dispensadores.Domain.Services;
using Backend_DispenXCore.Api.src.Dispensadores.Infrastructure.Persistence;
using Backend_DispenXCore.Api.src.Dispositivos.Application.Interfaces;
using Backend_DispenXCore.Api.src.Dispositivos.Application.UseCases;
using Backend_DispenXCore.Api.src.Dispositivos.Infrastructure.Persistence;
using Backend_DispenXCore.Api.src.NotificacionesUsuario.Application.Interfaces;
using Backend_DispenXCore.Api.src.NotificacionesUsuario.Application.UseCases;
using Backend_DispenXCore.Api.src.NotificacionesUsuario.Infrastructure.Persistence;
using Backend_DispenXCore.Api.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Backend_DispenXCore.Api.Shared.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<DispenXDbContext>(options =>
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

        // IAM
        services.AddScoped<IUserRepository, UserRepository>();
        // Usuarios (perfil)
        services.AddScoped<IPerfilRepository, PerfilRepository>();
        // Inventario
        services.AddScoped<IInventarioRepository, InventarioRepository>();
        // Alertas de stock
        services.AddScoped<IAlertaRepository, AlertaRepository>();
        // Dispensadores
        services.AddScoped<IDispenserRepository, DispenserRepository>();
        // Dispositivos
        services.AddScoped<IDeviceRepository, DeviceRepository>();
        // Notificaciones de usuario
        services.AddScoped<INotificationRepository, NotificationRepository>();

        return services;
    }

    public static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        services.AddScoped<PasswordHasherService>();
        services.AddScoped<CalculadorStockService>();
        services.AddScoped<EvaluadorUmbralService>();
        services.AddScoped<NextDispenseCalculator>();
        return services;
    }

    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // IAM
        services.AddScoped<ITokenService, JwtTokenService>();
        services.AddScoped<LoginCommand>();
        services.AddScoped<RegisterCommand>();

        // Perfil Usuario
        services.AddScoped<CrearPerfilCommand>();
        services.AddScoped<VincularDispensadorCommand>();

        // Inventario
        services.AddScoped<RegistrarMedicionCommand>();
        services.AddScoped<ObtenerEstadoGranoQuery>();

        // Alertas stock
        services.AddScoped<EvaluarAlertasCommand>();
        services.AddScoped<ObtenerAlertasQuery>();
        services.AddScoped<IPushService, PushNotificationService>();

        // Dispensadores
        services.AddScoped<ObtenerDispensatorsQuery>();
        services.AddScoped<ObtenerDispensatorStatusQuery>();
        services.AddScoped<AdministrarSchedulesCommand>();
        services.AddScoped<RegistrarEventoCommand>();

        // Dispositivos
        services.AddScoped<ObtenerDeviceQuery>();
        services.AddScoped<ActualizarDeviceCommand>();
        services.AddScoped<RegistrarPingCommand>();
        services.AddScoped<FirmwareUseCases>();

        // Notificaciones usuario
        services.AddScoped<ObtenerNotificacionesQuery>();
        services.AddScoped<MarcarNotificacionesCommand>();

        return services;
    }
}