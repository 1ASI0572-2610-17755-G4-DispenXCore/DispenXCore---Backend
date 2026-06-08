using Backend_DispenXCore.Api.src.AlertasStock.Application.Interfaces;

namespace Backend_DispenXCore.Api.src.AlertasStock.Infrastructure.Services;
public class PushNotificationService : IPushService
{
    public Task EnviarPushAsync(string mensaje, string deviceToken)
    {
        Console.WriteLine($"Push enviado a {deviceToken}: {mensaje}");
        return Task.CompletedTask;
    }
}