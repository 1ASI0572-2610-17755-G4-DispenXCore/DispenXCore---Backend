using Backend_DispenXCore.Api.src.Dispositivos.Application.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend_DispenXCore.Api.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/firmware")]
    [ApiController]
    [Authorize]
    public class FirmwareController : ControllerBase
    {
        private readonly FirmwareUseCases _firmware;

        public FirmwareController(FirmwareUseCases firmware) => _firmware = firmware;

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _firmware.GetAll());

        [HttpGet("latest")]
        public async Task<IActionResult> GetLatest()
        {
            var fw = await _firmware.GetLatest();
            return fw != null ? Ok(fw) : NotFound();
        }

        [HttpPost("{id}/install")]
        public async Task<IActionResult> Install(string id)
        {
            await _firmware.Install(id);
            return Ok(new { message = "Instalación iniciada" });
        }
    }
}