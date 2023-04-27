using Questao5.Domain.Entities;

namespace Questao5.Domain.Contracts;

public interface IContaCorrenteRepository
{
    Task<ContaCorrente> ObterPorNumeroContaCorrenteAsync(string numeroContaCorrente);
}