using Questao5.Domain.Enumerators.ContaCorrente;

namespace Questao5.Domain.Entities;

public class Movimento
{
    public string IdMovimento { get; set; }
    public string IdContaCorrente { get; set; }
    public virtual ContaCorrente ContaCorrente { get; set; }
    public string DataMovimento { get; set; }
    public TipoMovimentoEnum TipoMovimento { get; set; }
    public decimal Valor { get; set; }

    public Movimento()
    { }

    public Movimento(string idContaCorrente,
        string dataMovimento,
        TipoMovimentoEnum tipoMovimento,
        decimal valor)
    {
        IdContaCorrente = idContaCorrente;
        DataMovimento = dataMovimento;
        TipoMovimento = tipoMovimento;
        Valor = valor;
    }
}