using MediatR;
using Questao5.Application.Commands.Responses.ContaCorrente;
using Questao5.Domain.Enumerators.ContaCorrente;
using System.Text.Json.Serialization;

namespace Questao5.Application.Commands.Requests.ContaCorrente;

public sealed class CreateMovimentoCommand : IRequest<CreateMovimentoCommandResponse>
{
    [JsonIgnore]
    public Guid Chave_Idempotencia { get; set; }
    public string NumeroContaCorrente { get; set; }
    public TipoMovimentoEnum TipoMovimento { get; set; }
    public decimal Valor { get; set; }
}