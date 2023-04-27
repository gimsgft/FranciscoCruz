using MediatR;
using Questao5.Application.Commands.Requests.ContaCorrente;
using Questao5.Application.Commands.Responses.ContaCorrente;
using Questao5.Domain.Contracts;
using Questao5.Domain.Entities;
using Questao5.Domain.Enumerators.ContaCorrente;
using Questao5.Infrastructure.Exceptions;
using System.Text.Json;

namespace Questao5.Application.Commands.Handlers.ContaCorrente;

public sealed class CreateMovimentoCommandHandler : IRequestHandler<CreateMovimentoCommand, CreateMovimentoCommandResponse>
{
    private readonly ILogger<CreateMovimentoCommandHandler> _logger;
    private readonly IMovimentoRepository _movimentoRepository;
    private readonly IContaCorrenteRepository _contaCorrenteRepository;
    private readonly IIdemPotenciaRepository _idemPotenciaRepository;

    public CreateMovimentoCommandHandler(
        ILogger<CreateMovimentoCommandHandler> logger,
        IMovimentoRepository movimentoRepository,
        IContaCorrenteRepository contaCorrenteRepository,
        IIdemPotenciaRepository idemPotenciaRepository)
    {
        _logger = logger;
        _movimentoRepository = movimentoRepository;
        _contaCorrenteRepository = contaCorrenteRepository;
        _idemPotenciaRepository = idemPotenciaRepository;
    }

    public async Task<CreateMovimentoCommandResponse> Handle(CreateMovimentoCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Requisição método: {0} | body {1}", nameof(CreateMovimentoCommandHandler), JsonSerializer.Serialize(command));

        _logger.LogInformation("Chamando método _contaCorrenteRepository.ObterPorNumeroContaCorrenteAsync");
        var contaCorrente = await _contaCorrenteRepository.ObterPorNumeroContaCorrenteAsync(command.NumeroContaCorrente);

        _logger.LogInformation("Chamando método Validar");
        await ValidarAsync(command, contaCorrente);

        _logger.LogInformation("Chamando método _movimentoRepository.CriarAsync");
        var result = await _movimentoRepository.CriarAsync(new Movimento(contaCorrente.IdContaCorrente, DateTime.Now.ToString(), command.TipoMovimento, command.Valor));

        if(string.IsNullOrEmpty(result))
            throw new DomainException(new DomainExceptionBody("Movimento não criado", Domain.Enumerators.TipoErroEnum.DEFAULT_ERROR));

        _logger.LogInformation("Chamando método _idemPotenciaRepository.CriarAsync");
        await _idemPotenciaRepository.CriarAsync(command.Chave_Idempotencia.ToString());

        _logger.LogInformation("Finalizando requisição método: {0}", nameof(CreateMovimentoCommandHandler));
        return new CreateMovimentoCommandResponse(result);
    }

    private async Task ValidarAsync(CreateMovimentoCommand command, Domain.Entities.ContaCorrente contaCorrente)
    {
        if(await _idemPotenciaRepository.JaExisteAsync(command.Chave_Idempotencia.ToString()))
            throw new DomainException(new DomainExceptionBody("IdemPotencia - Transação já existe", Domain.Enumerators.TipoErroEnum.DEFAULT_ERROR));

        if (command.Valor <= 0)
            throw new DomainException(new DomainExceptionBody("Apenas valores positivos podem ser recebidos", Domain.Enumerators.TipoErroEnum.INVALID_VALUE));

        if (!Enum.IsDefined(typeof(TipoMovimentoEnum), command.TipoMovimento))
            throw new DomainException(new DomainExceptionBody("Apenas os tipos “débito” ou “crédito” podem ser aceitos", Domain.Enumerators.TipoErroEnum.INVALID_TYPE));

        if(contaCorrente == null || string.IsNullOrEmpty(contaCorrente.IdContaCorrente))
            throw new DomainException(new DomainExceptionBody("Apenas contas correntes cadastradas podem receber movimentação", Domain.Enumerators.TipoErroEnum.INVALID_ACCOUNT));
        else if (!contaCorrente.Ativo)
            throw new DomainException(new DomainExceptionBody("Apenas contas correntes ativas podem receber movimentação", Domain.Enumerators.TipoErroEnum.INACTIVE_ACCOUNT));
    }
}