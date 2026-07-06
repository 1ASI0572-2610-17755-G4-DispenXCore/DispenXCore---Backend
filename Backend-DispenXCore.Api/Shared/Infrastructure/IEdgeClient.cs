namespace Backend_DispenXCore.Api.Shared.Infrastructure
{
    public interface IEdgeClient
    {
        Task<bool> ActivateDispenserAsync(string deviceId, string? supplyType);
    }
}
