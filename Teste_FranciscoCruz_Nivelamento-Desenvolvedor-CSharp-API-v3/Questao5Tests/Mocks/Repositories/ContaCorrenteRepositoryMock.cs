using Moq;
using Questao5.Domain.Contracts;
using Questao5.Domain.Entities;

namespace Questao5Tests.Mocks.Repositories;

public sealed class ContaCorrenteRepositoryMock
{
    public readonly Mock<IContaCorrenteRepository> Instance;

    public ContaCorrenteRepositoryMock()
    {
        Instance = new Mock<IContaCorrenteRepository>();
    }

    public ContaCorrenteRepositoryMock SetupSuccessObterPorNumeroContaCorrente(string idContaCorrente, ContaCorrente expected)
    {
        Instance
            .Setup(x => x.ObterPorNumeroContaCorrenteAsync(idContaCorrente))
            .ReturnsAsync(expected);

        return this;
    }
}