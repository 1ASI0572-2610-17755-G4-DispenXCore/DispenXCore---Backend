using Backend_DispenXCore.Api.src.Inventario.Application.Interfaces;
using Backend_DispenXCore.Api.src.AlertasStock.Application.Interfaces;
using Backend_DispenXCore.Api.src.AlertasStock.Domain.Entities;
using Backend_DispenXCore.Api.src.AlertasStock.Domain.Services;

namespace Backend_DispenXCore.Api.src.AlertasStock.Application.UseCases;
public class EvaluarAlertasCommand
{
    private readonly IInventarioRepository _inventarioRepo;
    private readonly IAlertaRepository _alertaRepo;
    private readonly IPushService _pushService;
    private readonly EvaluadorUmbralService _evaluador;

    public EvaluarAlertasCommand(IInventarioRepository inventarioRepo,
        IAlertaRepository alertaRepo,
        IPushService pushService,
        EvaluadorUmbralService evaluador)
    {
        _inventarioRepo = inventarioRepo;
        _alertaRepo = alertaRepo;
        _pushService = pushService;
        _evaluador = evaluador;
    }

    public async Task Execute(Guid contenedorId, double umbral, string deviceToken)
    {
        var contenedor = await _inventarioRepo.GetByIdAsync(contenedorId);
        if (contenedor == null) return;

        if (_evaluador.DebeAlertar(contenedor.PorcentajeRestante, umbral))
        {
            var alerta = new Alerta(contenedorId, contenedor.Grano.Nombre,
                contenedor.PorcentajeRestante, umbral);
            await _alertaRepo.AddAsync(alerta);

            string mensaje = $"¡Aviso! Stock bajo de {contenedor.Grano.Nombre}: {contenedor.PorcentajeRestante:F1}%";
            await _pushService.EnviarPushAsync(mensaje, deviceToken);
            alerta.MarcarEnviada();
            await _alertaRepo.SaveChangesAsync();
        }
    }
}