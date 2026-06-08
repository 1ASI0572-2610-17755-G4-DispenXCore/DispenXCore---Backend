using Backend_DispenXCore.Api.src.AlertasStock.Application.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend_DispenXCore.Api.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/alertas-stock")]
    [ApiController]
    [Authorize]
    public class AlertasController : ControllerBase
    {
        private readonly EvaluarAlertasCommand _evaluar;
        private readonly ObtenerAlertasQuery _obtener;

        public AlertasController(EvaluarAlertasCommand evaluar, ObtenerAlertasQuery obtener)
        {
            _evaluar = evaluar;
            _obtener = obtener;
        }

        [HttpPost("evaluar")]
        public async Task<IActionResult> Evaluar(Guid contenedorId, double umbral, string deviceToken)
        {
            await _evaluar.Execute(contenedorId, umbral, deviceToken);
            return Ok();
        }

        [HttpGet("{contenedorId}")]
        public async Task<IActionResult> Obtener(Guid contenedorId)
        {
            var alertas = await _obtener.Execute(contenedorId);
            return Ok(alertas);
        }
    }
}