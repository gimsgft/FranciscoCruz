using Questao5.Domain.Entities;

namespace Questao5.Domain.Contracts;

public interface IIdemPotenciaRepository
{
    Task CriarAsync(string chave_Idempotencia);
    Task<bool> JaExisteAsync(string chaveIdempotencia);
}