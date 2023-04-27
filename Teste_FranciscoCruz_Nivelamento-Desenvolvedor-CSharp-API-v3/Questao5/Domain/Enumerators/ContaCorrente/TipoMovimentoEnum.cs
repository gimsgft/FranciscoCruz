using System.ComponentModel;

namespace Questao5.Domain.Enumerators.ContaCorrente;

public enum TipoMovimentoEnum
{
    [Description("Crédito")]
    C = 1,
    [Description("Débito")]
    D = 2
}