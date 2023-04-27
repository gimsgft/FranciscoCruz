using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Questao5.Application.Commands.Requests.ContaCorrente;
using Questao5.Application.Commands.Responses.ContaCorrente;
using Questao5.Application.Queries.Requests.ContaCorrente;
using Questao5.Application.Queries.Responses.ContaCorrente;
using Questao5.Infrastructure.CrossCutting.Constants;
using Questao5.Infrastructure.Exceptions;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using System.Text.Json;

namespace Questao5.Infrastructure.Services.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public sealed class ContaCorrenteController : ControllerBase
{
    private readonly ILogger<ContaCorrenteController> _logger;
    private readonly IMediator _mediator;
    public ContaCorrenteController(
        ILogger<ContaCorrenteController> logger,
        IMediator mediator)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpPost("{Chave_Idempotencia:guid}/movimentacao")]
    [Authorize]
    [SwaggerResponse((int)HttpStatusCode.OK, "", typeof(CreateMovimentoCommandResponse))]
    [SwaggerResponse((int)HttpStatusCode.BadRequest, "", typeof(DomainExceptionBody))]
    [SwaggerOperation(Summary = DescricoesSwagger.DescricaoContaCorrente.CriarMovimentacao)]
    public async Task<IActionResult> CriarMovimentacaoAsync([FromRoute] Guid Chave_Idempotencia, [FromBody] CreateMovimentoCommand command)
    {
        _logger.LogInformation("Requisição método: {0} | RequestBody {1}", nameof(CriarMovimentacaoAsync), JsonSerializer.Serialize(command));
        
        command.Chave_Idempotencia = Chave_Idempotencia;
        var response = await _mediator.Send(command);
        
        _logger.LogInformation("Resposta método: {0} | ResponseBody {1}", nameof(CriarMovimentacaoAsync), JsonSerializer.Serialize(response));
        return Ok(response);
    }

    [Authorize]
    [HttpGet("{numeroContaCorrente}/obter-saldo")]
    [SwaggerResponse((int)HttpStatusCode.OK, "", typeof(ObterSaldoQueryResponse))]
    [SwaggerResponse((int)HttpStatusCode.BadRequest, "", typeof(DomainExceptionBody))]
    [SwaggerOperation(Summary = DescricoesSwagger.DescricaoContaCorrente.ObterSaldo)]
    public async Task<IActionResult> ObterSaldoAsync([FromRoute] string numeroContaCorrente)
    {
        _logger.LogInformation("Requisição método: {0} | RequestBody {1}", nameof(ObterSaldoAsync), JsonSerializer.Serialize(numeroContaCorrente));
        
        var response = await _mediator.Send(new ObterSaldoQuery(numeroContaCorrente));

        _logger.LogInformation("Resposta método: {0} | ResponseBody {1}", nameof(ObterSaldoAsync), JsonSerializer.Serialize(response));
        return Ok(response);
    }
}