namespace Backend_DispenXCore.Api.src.Dispensadores.Domain.ValueObjects;

// Representa los días como una lista de enteros (0=domingo...6=sábado)
public record FrequencyDays(List<int> Days);