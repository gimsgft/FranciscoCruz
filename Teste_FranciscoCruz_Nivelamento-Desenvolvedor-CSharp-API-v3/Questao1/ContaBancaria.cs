using System.Globalization;

namespace Questao1;

class ContaBancaria
{
    #region Propriedades públicas
    public int Numero { get; }
    public string NomeTitular { get; set; }
    public double Valor { get; private set; }
    #endregion

    #region Constantes
    private const double TaxaSaque = 3.50;
    #endregion

    public ContaBancaria(int numero, string nomeTitular, double valor = 0)
    {
        Numero = numero;
        NomeTitular = nomeTitular;
        Valor = valor;
    }

    #region Métodos
    internal void Deposito(double quantia)
    {
        Valor += quantia;
    }

    internal void Saque(double quantia)
    {
        Valor = (Valor - quantia) - TaxaSaque;
    }

    public override string ToString()
    {
        return $"Conta {Numero}, Titular: {NomeTitular}, Saldo: $ {Valor.ToString("F", CultureInfo.InvariantCulture)}";
    }
    #endregion
}