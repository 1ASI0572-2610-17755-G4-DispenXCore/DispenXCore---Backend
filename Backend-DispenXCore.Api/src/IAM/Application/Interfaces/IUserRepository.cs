using Backend_DispenXCore.Api.src.IAM.Domain.Entities;

namespace Backend_DispenXCore.Api.src.IAM.Application.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByIdAsync(Guid id);       // ← nuevo método
    Task AddAsync(User user);
    Task SaveChangesAsync();
}