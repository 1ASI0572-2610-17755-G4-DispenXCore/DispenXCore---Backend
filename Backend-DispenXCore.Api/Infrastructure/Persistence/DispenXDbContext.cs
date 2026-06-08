using Backend_DispenXCore.Api.src.IAM.Domain.Entities;
using Backend_DispenXCore.Api.src.Inventario.Domain.Entities;
using Backend_DispenXCore.Api.src.AlertasStock.Domain.Entities;
using Backend_DispenXCore.Api.src.Usuarios.Domain.Entities;
using Backend_DispenXCore.Api.src.Dispensadores.Domain.Entities;
using Backend_DispenXCore.Api.src.Dispositivos.Domain.Entities;
using Backend_DispenXCore.Api.src.NotificacionesUsuario.Domain.Entities;
using Backend_DispenXCore.Api.Shared.Kernel;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Backend_DispenXCore.Api.src.Dispensadores.Domain.ValueObjects;

namespace Backend_DispenXCore.Api.Infrastructure.Persistence;

public class DispenXDbContext : DbContext, IUnitOfWork
{
    public DispenXDbContext(DbContextOptions<DispenXDbContext> options) : base(options) { }

    // IAM
    public DbSet<User> IamUsers => Set<User>();
    // Usuarios (perfil, dispensador)
    public DbSet<PerfilUsuario> PerfilesUsuarios => Set<PerfilUsuario>();
    public DbSet<Dispensador> Dispensadores => Set<Dispensador>();
    // Inventario
    public DbSet<Contenedor> Contenedores => Set<Contenedor>();
    public DbSet<DatoSensor> DatosSensor => Set<DatoSensor>();
    // Alertas stock
    public DbSet<Alerta> Alertas => Set<Alerta>();
    // Dispensadores (nuevo)
    public DbSet<Dispensator> Dispensators => Set<Dispensator>();
    public DbSet<DispensatorStatus> DispensatorStatuses => Set<DispensatorStatus>();
    public DbSet<Schedule> Schedules => Set<Schedule>();
    public DbSet<DispenserEvent> DispenserEvents => Set<DispenserEvent>();
    // Dispositivos
    public DbSet<Device> Devices => Set<Device>();
    public DbSet<Firmware> Firmwares => Set<Firmware>();
    // Notificaciones usuario
    public DbSet<Notification> Notifications => Set<Notification>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Ignore<FrequencyDays>();            // ← añadir esto
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}