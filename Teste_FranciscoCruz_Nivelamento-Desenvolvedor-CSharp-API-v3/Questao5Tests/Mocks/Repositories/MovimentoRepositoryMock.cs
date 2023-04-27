using Moq;
using Questao5.Domain.Contracts;
using Questao5.Domain.Entities;

namespace Questao5Tests.Mocks.Repositories;

public sealed class MovimentoRepositoryMock
{
    public readonly Mock<IMovimentoRepository> Instance;

    public MovimentoRepositoryMock()
    {
        Instance = new Mock<IMovimentoRepository>();
    }

    public MovimentoRepositoryMock SetupSuccessCriar(Movimento movimento, string? expected)
    {
        Instance
            .Setup(x => x.CriarAsync(movimento))
            .ReturnsAsync(expected);

        return this;
    }
}