namespace Questao5.Domain.Entities;

public class ContaCorrente
{
    public string IdContaCorrente { get; set; }
    public long Numero { get; set; }
    public string Nome { get; set; }
    public bool Ativo { get; set; }

    public ContaCorrente()
    { }

    public ContaCorrente(string idContaCorrente, long numero, string nome, bool ativo)
    {
        IdContaCorrente = idContaCorrente;
        Numero = numero;
        Nome = nome;
        Ativo = ativo;
    }
}