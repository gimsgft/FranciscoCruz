using Questao5.Domain.Enumerators;

namespace Questao5.Infrastructure.Exceptions;

public class DomainException : Exception
{
    public TipoErroEnum TipoErro { get; set; }
    public DomainException(DomainExceptionBody domainExceptionBody) : base(domainExceptionBody.Message)
    {
        TipoErro = domainExceptionBody.TipoErro;
    }
}

public class DomainExceptionBody
{
    public string Message { get; set; }
    public TipoErroEnum TipoErro { get; set; }

    public DomainExceptionBody(string message,
        TipoErroEnum tipoErro = TipoErroEnum.DEFAULT_ERROR)
    {
        Message = message;
        TipoErro = tipoErro;
    }
}