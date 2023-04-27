namespace Questao5.Domain.Entities;

public class IdemPotencia
{
    public string Chave_Idempotencia { get; set; }
    public string Requisicao { get; set; }
    public string Resultado { get; set; }

    public IdemPotencia(string chave_Idempotencia,
        string requisicao,
        string resultado)
    {
        Chave_Idempotencia = chave_Idempotencia;
        Requisicao = requisicao;
        Resultado = resultado;
    }
}