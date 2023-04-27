using AutoFixture;
using Microsoft.Extensions.Logging;
using Moq;
using Questao5.Application.Commands.Requests.ContaCorrente;
using Questao5.Domain.Contracts;
using Questao5.Domain.Enumerators.ContaCorrente;
using Questao5Tests.Mocks.Repositories;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Questao5.Application.Commands.Handlers.ContaCorrente.Tests
{
    public class CreateMovimentoCommandHandlerTests
    {
        readonly string idContaCorrente = "B6BAFC09-6967-ED11-A567-055DFA4A16C9";
        readonly string chaveIdempotencia = "e933222a-baa5-481d-be38-b61395c01fd7";

        [Fact]
        public void HandleTest_RetornaFalhaIdemPotencia()
        {
            var _logger = new Mock<ILogger<CreateMovimentoCommandHandler>>();
            var _movimentoRepository = new Mock<IMovimentoRepository>();
            _movimentoRepository.Setup(x => x.CriarAsync(It.IsAny<Domain.Entities.Movimento>())).Returns(Task.FromResult("1"));

            var expectedContaCorrente = new Fixture().Build<Domain.Entities.ContaCorrente>()
                .With(p => p.Ativo, true)
                .With(p => p.IdContaCorrente, idContaCorrente)
                .With(p => p.Nome, "Teste")
                .With(p => p.Numero, 123)
                .Create();
            var _contaCorrenteRepository = new ContaCorrenteRepositoryMock()
                .SetupSuccessObterPorNumeroContaCorrente(idContaCorrente, expectedContaCorrente)
                .Instance;

            var _idemPotenciaRepository = new Mock<IIdemPotenciaRepository>();
            _idemPotenciaRepository.Setup(x => x.JaExisteAsync(It.IsAny<string>())).Returns(Task.FromResult(true));

            var request = new Fixture().Build<CreateMovimentoCommand>()
                .With(o => o.Chave_Idempotencia, Guid.Parse(chaveIdempotencia))
                .With(o => o.NumeroContaCorrente, idContaCorrente)
                .With(o => o.TipoMovimento, TipoMovimentoEnum.C)
                .With(o => o.Valor, 1)
                .Create();

            var handler = new CreateMovimentoCommandHandler(
                _logger.Object,
                _movimentoRepository.Object,
                _contaCorrenteRepository.Object,
                _idemPotenciaRepository.Object
            );

            var ex = Assert.ThrowsAsync<Infrastructure.Exceptions.DomainException>(async () => await handler.Handle(request, default));

            Assert.Contains("IdemPotencia - Transação já existe", ex.Result.Message);
        }

        [Fact]
        public void HandleTest_RetornaFalhaValorMenorOuIgualZero()
        {
            var _logger = new Mock<ILogger<CreateMovimentoCommandHandler>>();
            var _movimentoRepository = new Mock<IMovimentoRepository>();
            _movimentoRepository.Setup(x => x.CriarAsync(It.IsAny<Domain.Entities.Movimento>())).Returns(Task.FromResult("1"));

            var expectedContaCorrente = new Fixture().Build<Domain.Entities.ContaCorrente>()
                .With(p => p.Ativo, true)
                .With(p => p.IdContaCorrente, idContaCorrente)
                .With(p => p.Nome, "Teste")
                .With(p => p.Numero, 123)
                .Create();
            var _contaCorrenteRepository = new ContaCorrenteRepositoryMock()
                .SetupSuccessObterPorNumeroContaCorrente(idContaCorrente, expectedContaCorrente)
                .Instance;

            var _idemPotenciaRepository = new Mock<IIdemPotenciaRepository>();

            var request = new Fixture().Build<CreateMovimentoCommand>()
                .With(o => o.Chave_Idempotencia, Guid.Parse(chaveIdempotencia))
                .With(o => o.NumeroContaCorrente, idContaCorrente)
                .With(o => o.TipoMovimento, TipoMovimentoEnum.C)
                .With(o => o.Valor, 0)
                .Create();

            var handler = new CreateMovimentoCommandHandler(
                _logger.Object,
                _movimentoRepository.Object,
                _contaCorrenteRepository.Object,
                _idemPotenciaRepository.Object
            );

            var ex = Assert.ThrowsAsync<Infrastructure.Exceptions.DomainException>(async () => await handler.Handle(request, default));

            Assert.Contains("Apenas valores positivos podem ser recebidos", ex.Result.Message);
        }

        [Fact]
        public void HandleTest_RetornaFalhaContaNaoLocalizada()
        {
            var _logger = new Mock<ILogger<CreateMovimentoCommandHandler>>();
            var _movimentoRepository = new Mock<IMovimentoRepository>();
            _movimentoRepository.Setup(x => x.CriarAsync(It.IsAny<Domain.Entities.Movimento>())).Returns(Task.FromResult("1"));

            var _contaCorrenteRepository = new ContaCorrenteRepositoryMock()
                .SetupSuccessObterPorNumeroContaCorrente(idContaCorrente, new Domain.Entities.ContaCorrente(string.Empty, 0, string.Empty, false))
                .Instance;

            var _idemPotenciaRepository = new Mock<IIdemPotenciaRepository>();

            var request = new Fixture().Build<CreateMovimentoCommand>()
                .With(o => o.Chave_Idempotencia, Guid.Parse(chaveIdempotencia))
                .With(o => o.NumeroContaCorrente, idContaCorrente)
                .With(o => o.TipoMovimento, TipoMovimentoEnum.C)
                .With(o => o.Valor, 1)
                .Create();

            var handler = new CreateMovimentoCommandHandler(
                _logger.Object,
                _movimentoRepository.Object,
                _contaCorrenteRepository.Object,
                _idemPotenciaRepository.Object
            );

            var ex = Assert.ThrowsAsync<Infrastructure.Exceptions.DomainException>(async () => await handler.Handle(request, default));

            Assert.Contains("Apenas contas correntes cadastradas podem receber movimentação", ex.Result.Message);
        }

        [Fact]
        public void HandleTest_RetornaFalhaContaInativa()
        {
            var _logger = new Mock<ILogger<CreateMovimentoCommandHandler>>();
            var _movimentoRepository = new Mock<IMovimentoRepository>();
            _movimentoRepository.Setup(x => x.CriarAsync(It.IsAny<Domain.Entities.Movimento>())).Returns(Task.FromResult("1"));

            var expectedContaCorrente = new Fixture().Build<Domain.Entities.ContaCorrente>()
                .With(p => p.Ativo, false)
                .With(p => p.IdContaCorrente, idContaCorrente)
                .With(p => p.Nome, "Teste")
                .With(p => p.Numero, 123)
                .Create();
            var _contaCorrenteRepository = new ContaCorrenteRepositoryMock()
                .SetupSuccessObterPorNumeroContaCorrente(idContaCorrente, expectedContaCorrente)
                .Instance;

            var _idemPotenciaRepository = new Mock<IIdemPotenciaRepository>();

            var request = new Fixture().Build<CreateMovimentoCommand>()
                .With(o => o.Chave_Idempotencia, Guid.Parse(chaveIdempotencia))
                .With(o => o.NumeroContaCorrente, idContaCorrente)
                .With(o => o.TipoMovimento, TipoMovimentoEnum.C)
                .With(o => o.Valor, 1)
                .Create();

            var handler = new CreateMovimentoCommandHandler(
                _logger.Object,
                _movimentoRepository.Object,
                _contaCorrenteRepository.Object,
                _idemPotenciaRepository.Object
            );

            var ex = Assert.ThrowsAsync<Infrastructure.Exceptions.DomainException>(async () => await handler.Handle(request, default));

            Assert.Contains("Apenas contas correntes ativas podem receber movimentação", ex.Result.Message);
        }

        [Fact]
        public void HandleTest_RetornaFalhaMovimentoNaoCriado()
        {
            var _logger = new Mock<ILogger<CreateMovimentoCommandHandler>>();
            var _movimentoRepository = new Mock<IMovimentoRepository>();
            _movimentoRepository.Setup(x => x.CriarAsync(It.IsAny<Domain.Entities.Movimento>())).Returns(Task.FromResult(string.Empty));

            var expectedContaCorrente = new Fixture().Build<Domain.Entities.ContaCorrente>()
                .With(p => p.Ativo, true)
                .With(p => p.IdContaCorrente, idContaCorrente)
                .With(p => p.Nome, "Teste")
                .With(p => p.Numero, 123)
                .Create();
            var _contaCorrenteRepository = new ContaCorrenteRepositoryMock()
                .SetupSuccessObterPorNumeroContaCorrente(idContaCorrente, expectedContaCorrente)
                .Instance;

            var _idemPotenciaRepository = new Mock<IIdemPotenciaRepository>();

            var request = new Fixture().Build<CreateMovimentoCommand>()
                .With(o => o.Chave_Idempotencia, Guid.Parse(chaveIdempotencia))
                .With(o => o.NumeroContaCorrente, idContaCorrente)
                .With(o => o.TipoMovimento, TipoMovimentoEnum.C)
                .With(o => o.Valor, 1)
                .Create();

            var handler = new CreateMovimentoCommandHandler(
                _logger.Object,
                _movimentoRepository.Object,
                _contaCorrenteRepository.Object,
                _idemPotenciaRepository.Object
            );

            var ex = Assert.ThrowsAsync<Infrastructure.Exceptions.DomainException>(async () => await handler.Handle(request, default));

            Assert.Contains("Movimento não criado", ex.Result.Message);
        }

        [Fact]
        public async Task HandleTest_RetornaSucesso()
        {
            var _logger = new Mock<ILogger<CreateMovimentoCommandHandler>>();
            var _movimentoRepository = new Mock<IMovimentoRepository>();
            _movimentoRepository.Setup(x => x.CriarAsync(It.IsAny<Domain.Entities.Movimento>())).Returns(Task.FromResult("1"));

            var expectedContaCorrente = new Fixture().Build<Domain.Entities.ContaCorrente>()
                .With(p => p.Ativo, true)
                .With(p => p.IdContaCorrente, idContaCorrente)
                .With(p => p.Nome, "Teste")
                .With(p => p.Numero, 123)
                .Create();
            var _contaCorrenteRepository = new ContaCorrenteRepositoryMock()
                .SetupSuccessObterPorNumeroContaCorrente(idContaCorrente, expectedContaCorrente)
                .Instance;

            var _idemPotenciaRepository = new Mock<IIdemPotenciaRepository>();

            var request = new Fixture().Build<CreateMovimentoCommand>()
                .With(o => o.Chave_Idempotencia, Guid.Parse(chaveIdempotencia))
                .With(o => o.NumeroContaCorrente, idContaCorrente)
                .With(o => o.TipoMovimento, TipoMovimentoEnum.C)
                .With(o => o.Valor, 1)
                .Create();

            var handler = new CreateMovimentoCommandHandler(
                _logger.Object,
                _movimentoRepository.Object,
                _contaCorrenteRepository.Object,
                _idemPotenciaRepository.Object
            );

            var response = await handler.Handle(request, default);

            Assert.Equal(new Responses.ContaCorrente.CreateMovimentoCommandResponse("1").MovimentoId, response.MovimentoId);
        }
    }
}