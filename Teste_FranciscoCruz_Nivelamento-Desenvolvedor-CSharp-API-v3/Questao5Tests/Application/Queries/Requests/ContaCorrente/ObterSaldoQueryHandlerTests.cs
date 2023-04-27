using AutoFixture;
using Microsoft.Extensions.Logging;
using Moq;
using Questao5.Application.Queries.Responses.ContaCorrente;
using Questao5.Domain.Contracts;
using Questao5.Domain.Enumerators.ContaCorrente;
using Questao5Tests.Mocks.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Questao5.Application.Queries.Requests.ContaCorrente.Tests
{
    public class ObterSaldoQueryHandlerTests
    {
        long numeroContaCorrente = 123;
        string idContaCorrente = "B6BAFC09-6967-ED11-A567-055DFA4A16C9";

        [Fact]
        public void HandleTest_RetornaFalhaContaNaoLocalizada()
        {
            List<Domain.Entities.Movimento> movimentos = new List<Domain.Entities.Movimento>()
            {
                new Domain.Entities.Movimento() { IdMovimento = "1", IdContaCorrente = idContaCorrente, TipoMovimento = TipoMovimentoEnum.C, Valor = 3 },
                new Domain.Entities.Movimento() { IdMovimento = "1", IdContaCorrente = idContaCorrente, TipoMovimento = TipoMovimentoEnum.D, Valor = 2 },
            };

            var _logger = new Mock<ILogger<ObterSaldoQueryHandler>>();
            var _movimentoRepository = new Mock<IMovimentoRepository>();
            _movimentoRepository.Setup(x => x.ObterMovimentosAsync(numeroContaCorrente.ToString())).Returns(Task.FromResult(movimentos));

            var expectedContaCorrente = new Fixture().Build<Domain.Entities.ContaCorrente>()
                .With(p => p.Ativo, true)
                .With(p => p.IdContaCorrente, idContaCorrente)
                .With(p => p.Nome, "Teste")
                .With(p => p.Numero, numeroContaCorrente)
                .Create();
            var _contaCorrenteRepository = new ContaCorrenteRepositoryMock()
                .SetupSuccessObterPorNumeroContaCorrente(numeroContaCorrente.ToString(), new Domain.Entities.ContaCorrente())
                .Instance;

            var responseExpected = new Fixture().Build<ObterSaldoQueryResponse>()
                .With(p => p.ContaCorrenteNumero, numeroContaCorrente)
                .With(p => p.ContaCorrenteNomeTitular, "Teste")
                .With(p => p.ValorSaldoAtual, 1)
                .Create();

            var request = new ObterSaldoQuery(numeroContaCorrente.ToString());

            var handler = new ObterSaldoQueryHandler(
                _logger.Object,
                _contaCorrenteRepository.Object,
                _movimentoRepository.Object
            );

            var ex = Assert.ThrowsAsync<Infrastructure.Exceptions.DomainException>(async () => await handler.Handle(request, default));

            Assert.Contains("Apenas contas correntes cadastradas podem consultar o saldo", ex.Result.Message);
        }

        [Fact]
        public void HandleTest_RetornaFalhaContaInativa()
        {
            List<Domain.Entities.Movimento> movimentos = new List<Domain.Entities.Movimento>()
            {
                new Domain.Entities.Movimento() { IdMovimento = "1", IdContaCorrente = idContaCorrente, TipoMovimento = TipoMovimentoEnum.C, Valor = 3 },
                new Domain.Entities.Movimento() { IdMovimento = "1", IdContaCorrente = idContaCorrente, TipoMovimento = TipoMovimentoEnum.D, Valor = 2 },
            };

            var _logger = new Mock<ILogger<ObterSaldoQueryHandler>>();
            var _movimentoRepository = new Mock<IMovimentoRepository>();
            _movimentoRepository.Setup(x => x.ObterMovimentosAsync(numeroContaCorrente.ToString())).Returns(Task.FromResult(movimentos));

            var expectedContaCorrente = new Fixture().Build<Domain.Entities.ContaCorrente>()
                .With(p => p.Ativo, false)
                .With(p => p.IdContaCorrente, idContaCorrente)
                .With(p => p.Nome, "Teste")
                .With(p => p.Numero, numeroContaCorrente)
                .Create();
            var _contaCorrenteRepository = new ContaCorrenteRepositoryMock()
                .SetupSuccessObterPorNumeroContaCorrente(numeroContaCorrente.ToString(), expectedContaCorrente)
                .Instance;

            var responseExpected = new Fixture().Build<ObterSaldoQueryResponse>()
                .With(p => p.ContaCorrenteNumero, numeroContaCorrente)
                .With(p => p.ContaCorrenteNomeTitular, "Teste")
                .With(p => p.ValorSaldoAtual, 1)
                .Create();

            var request = new ObterSaldoQuery(numeroContaCorrente.ToString());

            var handler = new ObterSaldoQueryHandler(
                _logger.Object,
                _contaCorrenteRepository.Object,
                _movimentoRepository.Object
            );

            var ex = Assert.ThrowsAsync<Infrastructure.Exceptions.DomainException>(async () => await handler.Handle(request, default));

            Assert.Contains("Apenas contas correntes ativas podem consultar o saldo", ex.Result.Message);
        }

        [Fact]
        public async Task HandleTest_RetornaClienteComSaldoAsync()
        {
            List<Domain.Entities.Movimento> movimentos = new List<Domain.Entities.Movimento>()
            {
                new Domain.Entities.Movimento() { IdMovimento = "1", IdContaCorrente = idContaCorrente, TipoMovimento = TipoMovimentoEnum.C, Valor = 3 },
                new Domain.Entities.Movimento() { IdMovimento = "1", IdContaCorrente = idContaCorrente, TipoMovimento = TipoMovimentoEnum.D, Valor = 2 },
            };

            var _logger = new Mock<ILogger<ObterSaldoQueryHandler>>();
            var _movimentoRepository = new Mock<IMovimentoRepository>();
            _movimentoRepository.Setup(x => x.ObterMovimentosAsync(numeroContaCorrente.ToString())).Returns(Task.FromResult(movimentos));

            var expectedContaCorrente = new Fixture().Build<Domain.Entities.ContaCorrente>()
                .With(p => p.Ativo, true)
                .With(p => p.IdContaCorrente, idContaCorrente)
                .With(p => p.Nome, "Teste")
                .With(p => p.Numero, numeroContaCorrente)
                .Create();
            var _contaCorrenteRepository = new ContaCorrenteRepositoryMock()
                .SetupSuccessObterPorNumeroContaCorrente(numeroContaCorrente.ToString(), expectedContaCorrente)
                .Instance;

            var responseExpected = new Fixture().Build<ObterSaldoQueryResponse>()
                .With(p => p.ContaCorrenteNumero, numeroContaCorrente)
                .With(p => p.ContaCorrenteNomeTitular, "Teste")
                .With(p => p.ValorSaldoAtual, 1)
                .Create();

            var request = new ObterSaldoQuery(numeroContaCorrente.ToString());

            var handler = new ObterSaldoQueryHandler(
                _logger.Object,
                _contaCorrenteRepository.Object,
                _movimentoRepository.Object
            );

            var response = await handler.Handle(request, default);

            Assert.Equal(responseExpected.ValorSaldoAtual, response.ValorSaldoAtual);
            Assert.Equal(1, response.ValorSaldoAtual);
        }
    }
}