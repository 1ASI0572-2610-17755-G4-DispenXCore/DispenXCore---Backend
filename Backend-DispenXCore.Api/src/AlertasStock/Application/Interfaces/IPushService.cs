namespace Backend_DispenXCore.Api.src.AlertasStock.Application.Interfaces;
public interface IPushService
{
    Task EnviarPushAsync(string mensaje, string deviceToken);
}