namespace Questao5.Application.Commands.Responses.ContaCorrente;

public class CreateMovimentoCommandResponse
{
    public string MovimentoId { get; set; }

    public CreateMovimentoCommandResponse(string movimentoId)
    {
        MovimentoId = movimentoId;
    }
}