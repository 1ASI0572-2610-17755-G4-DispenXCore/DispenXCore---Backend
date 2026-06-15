using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Backend_DispenXCore.Api.src.Dispensadores.Application.Interfaces;
using Backend_DispenXCore.Api.src.Dispensadores.Application.UseCases;
using Backend_DispenXCore.Api.src.Dispensadores.Domain.Entities;

namespace Backend_DispenXCore.Api.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/dispensators")]
    [ApiController]
    [Authorize]
    public class DispensatorsController : ControllerBase
    {
        private readonly ObtenerDispensatorsQuery _getAll;
        private readonly ObtenerDispensatorStatusQuery _getStatus;
        private readonly IDispenserRepository _repo;

        public DispensatorsController(
            ObtenerDispensatorsQuery getAll,
            ObtenerDispensatorStatusQuery getStatus,
            IDispenserRepository repo)
        {
            _getAll = getAll;
            _getStatus = getStatus;
            _repo = repo;
        }

        /// <summary>
        /// Obtiene la lista de todos los dispensadores registrados.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var dispensadores = await _getAll.Execute();
            return Ok(dispensadores);
        }

        /// <summary>
        /// Obtiene el estado detallado de un dispensador específico.
        /// Calcula dinámicamente nextDispenseAt y dailyTotal.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var status = await _getStatus.Execute(id);
            if (status == null)
                return NotFound(new { message = $"No se encontró el dispensador con ID {id}" });

            return Ok(status);
        }

        /// <summary>
        /// Crea un nuevo dispensador y su estado inicial.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateDispensatorRequest request)
        {
            var dispensator = new Dispensator(request.Name, request.MaxCapacity);

            // Si se especifica un estado, lo asignamos
            if (!string.IsNullOrEmpty(request.Status) && 
                Enum.TryParse<DispensatorState>(request.Status.ToLower(), out var state))
            {
                // Usamos reflexión para cambiar el estado (o podrías agregar un método SetStatus)
                typeof(Dispensator)
                    .GetProperty("Status")?
                    .SetValue(dispensator, state);
            }

            await _repo.AddDispensatorAsync(dispensator);
            await _repo.SaveChangesAsync();

            var status = new DispensatorStatus(dispensator.Id, request.MaxCapacity);
            await _repo.AddDispensatorStatusAsync(status);
            await _repo.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = dispensator.Id }, new
            {
                id = dispensator.Id,
                name = dispensator.Name,
                status = dispensator.Status.ToString(),
                maxCapacity = dispensator.MaxCapacity,
                message = "Dispensador creado correctamente"
            });
        }
    }

    public record CreateDispensatorRequest(string Name, int MaxCapacity, string? Status = null);
}