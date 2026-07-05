using Backend_DispenXCore.Api.src.Dispositivos.Application.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend_DispenXCore.Api.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/device")]
    [ApiController]
    [Authorize]
    public class DeviceController : ControllerBase
    {
        private readonly ObtenerDeviceQuery _getDevice;
        private readonly ActualizarDeviceCommand _update;
        private readonly RegistrarPingCommand _ping;
        private readonly RegistrarDeviceCommand _register;

        public DeviceController(ObtenerDeviceQuery getDevice, ActualizarDeviceCommand update, RegistrarPingCommand ping, RegistrarDeviceCommand register)
        {
            _getDevice = getDevice;
            _update = update;
            _ping = ping;
            _register = register;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var device = await _getDevice.Execute();
            return device != null ? Ok(device) : NotFound();
        }

        [HttpPatch]
        public async Task<IActionResult> Update([FromBody] DeviceUpdateDto dto)
        {
            await _update.Execute(dto.Name, dto.Location);
            return NoContent();
        }

        [HttpPost("ping")]
        [AllowAnonymous]
        public async Task<IActionResult> Ping()
        {
            await _ping.Execute();
            return Ok(new { message = "Ping registrado" });
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] DeviceRegistrationDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.DeviceId))
                return BadRequest(new { message = "device_id es requerido" });

            await _register.Execute(dto.DeviceId, dto.MacAddress, dto.IpAddress);
            return Ok(new { message = "Dispositivo registrado en el backend", device_id = dto.DeviceId });
        }
    }

    public record DeviceUpdateDto(string Name, string Location);
    public class DeviceRegistrationDto
    {
        [System.Text.Json.Serialization.JsonPropertyName("device_id")]
        public string? DeviceId { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("mac_address")]
        public string? MacAddress { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("ip_address")]
        public string? IpAddress { get; set; }
    }
}