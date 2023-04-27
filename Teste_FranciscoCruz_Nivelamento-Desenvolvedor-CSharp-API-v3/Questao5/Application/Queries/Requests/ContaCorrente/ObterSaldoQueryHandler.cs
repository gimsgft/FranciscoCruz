using MediatR;
using Questao5.Application.Queries.Responses.ContaCorrente;
using Questao5.Domain.Contracts;
using Questao5.Domain.Enumerators.ContaCorrente;
using Questao5.Infrastructure.Exceptions;
using System.Text.Json;

namespace Questao5.Application.Queries.Requests.ContaCorrente;

public sealed class ObterSaldoQueryHandler : IRequestHandler<ObterSaldoQuery, ObterSaldoQueryResponse>
{
    private readonly ILogger<ObterSaldoQueryHandler> _logger;
    private readonly IContaCorrenteRepository _contaCorrenteRepository;
    private readonly IMovimentoRepository _movimentoRepository;

    public ObterSaldoQueryHandler(
        ILogger<ObterSaldoQueryHandler> logger,
        IContaCorrenteRepository contaCorrenteRepository,
        IMovimentoRepository movimentoRepository)
    {
        _logger = logger;
        _movimentoRepository = movimentoRepository;
        _contaCorrenteRepository = contaCorrenteRepository;
    }

    public async Task<ObterSaldoQueryResponse> Handle(ObterSaldoQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Requisição método: {0} | body {1}", nameof(ObterSaldoQueryHandler), JsonSerializer.Serialize(request));

        _logger.LogInformation("Chamando método _contaCorrenteRepository.ObterPorNumeroContaCorrenteAsync");
        var contaCorrente = await _contaCorrenteRepository.ObterPorNumeroContaCorrenteAsync(request.NumeroContaCorrente);

        _logger.LogInformation("Chamando método Validar");
        Validar(contaCorrente);

        _logger.LogInformation("Chamando método _movimentoRepository.ObterMovimentosAsync");
        var movimentos = await _movimentoRepository.ObterMovimentosAsync(request.NumeroContaCorrente);

        _logger.LogInformation("Calculando saldo atual");
        var saldoAtual = movimentos.GroupBy(
                y => y.IdContaCorrente,
                y => (y.TipoMovimento, y.Valor),
                (key, elements) => new {
                    IdContaCorrente = key,
                    Total = elements.Sum(el => el.TipoMovimento == TipoMovimentoEnum.C ? el.Valor : -el.Valor)
                }).FirstOrDefault();

        _logger.LogInformation("Finalizando requisição método: {0}", nameof(ObterSaldoQueryHandler));
        return new ObterSaldoQueryResponse(contaCorrente.Numero, contaCorrente.Nome, DateTime.Now, saldoAtual?.Total ?? 0);
    }

    private static void Validar(Domain.Entities.ContaCorrente contaCorrente)
    {
        if (contaCorrente == null || string.IsNullOrEmpty(contaCorrente.IdContaCorrente))
            throw new DomainException(new DomainExceptionBody("Apenas contas correntes cadastradas podem consultar o saldo", Domain.Enumerators.TipoErroEnum.INVALID_ACCOUNT));
        else if (!Convert.ToBoolean(contaCorrente.Ativo))
            throw new DomainException(new DomainExceptionBody("Apenas contas correntes ativas podem consultar o saldo", Domain.Enumerators.TipoErroEnum.INACTIVE_ACCOUNT));
    }
}