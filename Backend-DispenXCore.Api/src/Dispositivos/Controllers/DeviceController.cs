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

        public DeviceController(ObtenerDeviceQuery getDevice, ActualizarDeviceCommand update, RegistrarPingCommand ping)
        {
            _getDevice = getDevice;
            _update = update;
            _ping = ping;
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
        public async Task<IActionResult> Ping()
        {
            await _ping.Execute();
            return Ok(new { message = "Ping registrado" });
        }
    }

    public record DeviceUpdateDto(string Name, string Location);
}