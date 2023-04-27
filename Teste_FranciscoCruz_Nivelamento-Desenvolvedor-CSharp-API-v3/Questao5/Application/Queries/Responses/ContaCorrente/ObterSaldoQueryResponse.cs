namespace Questao5.Application.Queries.Responses.ContaCorrente;

public class ObterSaldoQueryResponse
{
    public long ContaCorrenteNumero { get; set; }
    public string ContaCorrenteNomeTitular { get; set; }
    public DateTime DataHoraRespostaConsulta { get; set; }
    public decimal ValorSaldoAtual { get; set; }

    public ObterSaldoQueryResponse(long contaCorrenteNumero,
        string contaCorrenteNomeTitular,
        DateTime dataHoraRespostaConsulta,
        decimal valorSaldoAtual)
    {
        ContaCorrenteNumero = contaCorrenteNumero;
        ContaCorrenteNomeTitular = contaCorrenteNomeTitular;
        DataHoraRespostaConsulta = dataHoraRespostaConsulta;
        ValorSaldoAtual = valorSaldoAtual;
    }
}