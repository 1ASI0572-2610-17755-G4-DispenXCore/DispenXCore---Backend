using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Backend_DispenXCore.Api.src.Dispensadores.Application.Interfaces;
using Backend_DispenXCore.Api.src.Dispensadores.Application.UseCases;

namespace Backend_DispenXCore.Api.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/dispenser-events")]
    [ApiController]
    public class DispenserEventsController : ControllerBase
    {
        private readonly IDispenserRepository _repo;
        private readonly RegistrarEventoCommand _registrar;

        public DispenserEventsController(
            IDispenserRepository repo,
            RegistrarEventoCommand registrar)
        {
            _repo = repo;
            _registrar = registrar;
        }

        /// <summary>
        /// Obtiene el historial de eventos de dispensación.
        /// Se puede filtrar por rango de fechas y tipo de suministro.
        /// </summary>
        /// <param name="dispensatorId">ID del dispensador (obligatorio).</param>
        /// <param name="from">Fecha de inicio del filtro (opcional).</param>
        /// <param name="to">Fecha de fin del filtro (opcional).</param>
        /// <param name="supplyType">Tipo de suministro: RICE, LENTILS, BEANS, CORN, OTHER (opcional).</param>
        /// <returns>Lista de eventos de dispensación.</returns>
        [HttpGet]
        public async Task<IActionResult> GetEvents(
            [FromQuery] int dispensatorId,
            [FromQuery] DateTime? from = null,
            [FromQuery] DateTime? to = null,
            [FromQuery] string? supplyType = null)
        {
            var events = await _repo.GetEventsAsync(dispensatorId, from, to, supplyType);
            return Ok(events);
        }

        /// <summary>
        /// Registra un nuevo evento de dispensación (manual o automático).
        /// Endpoint público (sin autenticación) para que el Edge Service / ESP32
        /// pueda reportar dispensaciones directamente desde la red local.
        /// </summary>
        /// <param name="request">Datos del evento de dispensación.</param>
        /// <returns>200 OK si se registró correctamente.</returns>
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> PostEvent([FromBody] DispenserEventRequest request)
        {
            await _registrar.Execute(
                request.DispensatorId,
                request.ScheduleId,
                request.Trigger,
                request.SupplyType,
                request.AmountDispensed,
                request.DispensedAt);

            return Ok(new { message = "Evento registrado correctamente" });
        }
    }

    /// <summary>
    /// DTO para registrar un evento de dispensación.
    /// </summary>
    public record DispenserEventRequest(
        int DispensatorId,
        int? ScheduleId,
        string Trigger,
        string? SupplyType,
        int AmountDispensed,
        DateTime DispensedAt);
}