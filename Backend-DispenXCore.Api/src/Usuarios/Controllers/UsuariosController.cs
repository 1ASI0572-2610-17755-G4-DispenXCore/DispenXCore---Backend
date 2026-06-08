using Backend_DispenXCore.Api.src.IAM.Application.Interfaces;
using Backend_DispenXCore.Api.src.IAM.Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Backend_DispenXCore.Api.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/users")]
    [ApiController]
    [Authorize]
    public class UsuariosController : ControllerBase
    {
        private readonly IUserRepository _userRepo;
        private readonly PasswordHasherService _hasher;

        public UsuariosController(IUserRepository userRepo, PasswordHasherService hasher)
        {
            _userRepo = userRepo;
            _hasher = hasher;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(Guid id)
        {
            var user = await _userRepo.GetByIdAsync(id);
            if (user == null) return NotFound();
            return Ok(new {
                id = user.Id,
                firstName = user.FirstName,
                lastName = user.LastName,
                email = user.Email,
                role = user.Role.ToString(),
                status = user.Status.ToString(),
                photoUrl = user.PhotoUrl
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UpdateUserRequest request)
        {
            var user = await _userRepo.GetByIdAsync(id);
            if (user == null) return NotFound();
            user.UpdateProfile(request.FirstName, request.LastName, request.PhotoUrl);
            await _userRepo.SaveChangesAsync();
            return NoContent();
        }

        [HttpPatch("{id}/password")]
        public async Task<IActionResult> ChangePassword(Guid id, [FromBody] ChangePasswordRequest request)
        {
            var user = await _userRepo.GetByIdAsync(id);
            if (user == null) return NotFound();
            if (!_hasher.VerifyPassword(request.CurrentPassword, user.PasswordHash))
                return BadRequest(new { message = "Contraseña actual incorrecta" });

            var newHash = _hasher.HashPassword(request.NewPassword);
            user.ChangePassword(newHash);
            await _userRepo.SaveChangesAsync();
            return NoContent();
        }
    }

    public record UpdateUserRequest(string FirstName, string LastName, string? PhotoUrl);
    public record ChangePasswordRequest(string CurrentPassword, string NewPassword);
}