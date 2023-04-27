using Questao5.Domain.Entities;

namespace Questao5.Domain.Contracts;

public interface IMovimentoRepository
{
    Task<string> CriarAsync(Movimento movimento);
    Task<List<Movimento>> ObterMovimentosAsync(string idContaCorrente);
}