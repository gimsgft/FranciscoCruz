using MediatR;
using Questao5.Application.Queries.Responses.ContaCorrente;

namespace Questao5.Application.Queries.Requests.ContaCorrente;

public sealed class ObterSaldoQuery : IRequest<ObterSaldoQueryResponse>
{
    public string NumeroContaCorrente { get; set; }

    public ObterSaldoQuery(string numeroContaCorrente)
    {
        NumeroContaCorrente = numeroContaCorrente;
    }
}