using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Backend_DispenXCore.Api.src.IAM.Application.UseCases;

namespace Backend_DispenXCore.Api.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly RegisterCommand _register;
        private readonly LoginCommand _login;

        public AuthController(RegisterCommand register, LoginCommand login)
        {
            _register = register;
            _login = login;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            await _register.Execute(request.FirstName, request.LastName, request.Email, request.Password);
            return Ok(new { message = "Usuario registrado correctamente" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var (token, user) = await _login.Execute(request.Email, request.Password);
            if (token == null)
                return Unauthorized();

            return Ok(new
            {
                token,
                user = new
                {
                    id = user!.Id,
                    firstName = user.FirstName,
                    lastName = user.LastName,
                    email = user.Email,
                    role = user.Role.ToString(),
                    status = user.Status.ToString(),
                    photoUrl = user.PhotoUrl
                }
            });
        }

        [HttpPost("logout")]
        [Authorize]
        public IActionResult Logout()
        {
            // JWT es stateless, simplemente devolvemos OK
            return Ok(new { message = "Sesión cerrada" });
        }
    }

    public record RegisterRequest(string FirstName, string LastName, string Email, string Password);
    public record LoginRequest(string Email, string Password);
}