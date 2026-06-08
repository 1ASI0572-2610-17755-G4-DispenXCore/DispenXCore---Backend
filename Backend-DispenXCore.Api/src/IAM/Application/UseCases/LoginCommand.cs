using Backend_DispenXCore.Api.src.IAM.Application.Interfaces;
using Backend_DispenXCore.Api.src.IAM.Domain.Entities;
using Backend_DispenXCore.Api.src.IAM.Domain.Services;

namespace Backend_DispenXCore.Api.src.IAM.Application.UseCases;

public class LoginCommand
{
    private readonly IUserRepository _repo;
    private readonly PasswordHasherService _hasher;
    private readonly ITokenService _tokenService;

    public LoginCommand(IUserRepository repo, PasswordHasherService hasher, ITokenService tokenService)
    {
        _repo = repo;
        _hasher = hasher;
        _tokenService = tokenService;
    }

    public async Task<(string? Token, User? User)> Execute(string email, string password)
    {
        var user = await _repo.GetByEmailAsync(email);
        if (user == null || !_hasher.VerifyPassword(password, user.PasswordHash))
            return (null, null);

        var token = _tokenService.GenerateToken(user);
        return (token, user);
    }
}