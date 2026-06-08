using Backend_DispenXCore.Api.src.NotificacionesUsuario.Application.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Backend_DispenXCore.Api.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/notifications")]
    [ApiController]
    [Authorize]
    public class NotificationsController : ControllerBase
    {
        private readonly ObtenerNotificacionesQuery _getNotifs;
        private readonly MarcarNotificacionesCommand _marcar;

        public NotificationsController(ObtenerNotificacionesQuery getNotifs, MarcarNotificacionesCommand marcar)
        {
            _getNotifs = getNotifs;
            _marcar = marcar;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] Guid userId)
        {
            var notifs = await _getNotifs.Execute(userId);
            return Ok(notifs);
        }

        [HttpPatch("{id}/read")]
        public async Task<IActionResult> MarkRead(Guid id)
        {
            await _marcar.MarkAsRead(id);
            return NoContent();
        }

        [HttpPatch("read-all")]
        public async Task<IActionResult> MarkAllRead([FromQuery] Guid userId)
        {
            await _marcar.MarkAllAsRead(userId);
            return NoContent();
        }
    }
}