using Backend_DispenXCore.Api.src.Inventario.Application.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace Backend_DispenXCore.Api.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/inventario")]
    [ApiController]
    [Authorize]
    public class InventarioController : ControllerBase
    {
        private readonly RegistrarMedicionCommand _registrarMedicion;
        private readonly ObtenerEstadoGranoQuery _obtenerEstado;

        public InventarioController(RegistrarMedicionCommand registrarMedicion,
            ObtenerEstadoGranoQuery obtenerEstado)
        {
            _registrarMedicion = registrarMedicion;
            _obtenerEstado = obtenerEstado;
        }

        [HttpPost("medicion")]
        [AllowAnonymous]
        public async Task<IActionResult> RegistrarMedicion([FromBody] RegistrarMedicionRequest? request)
        {
            if (request is null)
            {
                request = new RegistrarMedicionRequest
                {
                    DeviceId = Request.Query["device_id"].FirstOrDefault() ?? Request.Query["deviceId"].FirstOrDefault(),
                    ContenedorId = Guid.TryParse(Request.Query["contenedorId"].FirstOrDefault(), out var parsedContainerId) ? parsedContainerId : null,
                    Readings = new Dictionary<string, double?>
                    {
                        ["weight"] = ParseDouble(Request.Query["peso"].FirstOrDefault()) ?? ParseDouble(Request.Query["weight"].FirstOrDefault()),
                        ["level"] = ParseDouble(Request.Query["nivel"].FirstOrDefault()) ?? ParseDouble(Request.Query["level"].FirstOrDefault()),
                        ["flow"] = ParseDouble(Request.Query["flujo"].FirstOrDefault()) ?? ParseDouble(Request.Query["flow"].FirstOrDefault())
                    }
                };
            }

            if (request.Readings is null || !request.Readings.Any())
                return BadRequest(new { message = "Se requiere un objeto readings con al menos un valor." });

            var peso = GetReadingValue(request.Readings, "weight", "peso") ?? 0;
            var nivel = GetReadingValue(request.Readings, "level", "nivel") ?? 0;
            var flujo = GetReadingValue(request.Readings, "flow", "flujo") ?? 0;

            await _registrarMedicion.Execute(request.DeviceId, request.ContenedorId, peso, nivel, flujo);
            return Ok(new { status = "success", device_id = request.DeviceId, contenedor_id = request.ContenedorId });
        }

        [HttpGet("estado")]
        public async Task<IActionResult> ObtenerEstado()
        {
            var resultado = await _obtenerEstado.Execute();
            return Ok(resultado);
        }

        private static double? GetReadingValue(Dictionary<string, double?> readings, params string[] keys)
        {
            foreach (var key in keys)
            {
                if (readings.TryGetValue(key, out var value) && value.HasValue)
                    return value.Value;
            }

            return null;
        }

        private static double? ParseDouble(string? value)
        {
            return double.TryParse(value, out var parsedValue) ? parsedValue : null;
        }
    }

    public class RegistrarMedicionRequest
    {
        [JsonPropertyName("device_id")]
        public string? DeviceId { get; set; }

        [JsonPropertyName("container_id")]
        public Guid? ContenedorId { get; set; }

        [JsonPropertyName("readings")]
        public Dictionary<string, double?>? Readings { get; set; }
    }
}