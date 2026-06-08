using Backend_DispenXCore.Api.src.IAM.Application.Interfaces;
using Backend_DispenXCore.Api.src.IAM.Domain.Entities;
using Backend_DispenXCore.Api.src.IAM.Domain.Services;

namespace Backend_DispenXCore.Api.src.IAM.Application.UseCases;

public class RegisterCommand
{
    private readonly IUserRepository _repo;
    private readonly PasswordHasherService _hasher;

    public RegisterCommand(IUserRepository repo, PasswordHasherService hasher)
    {
        _repo = repo;
        _hasher = hasher;
    }

    public async Task Execute(string firstName, string lastName, string email, string password)
    {
        var existente = await _repo.GetByEmailAsync(email);
        if (existente != null)
            throw new InvalidOperationException("El email ya está registrado");

        var hash = _hasher.HashPassword(password);
        var user = new User(firstName, lastName, email, hash);
        await _repo.AddAsync(user);
        await _repo.SaveChangesAsync();
    }
}