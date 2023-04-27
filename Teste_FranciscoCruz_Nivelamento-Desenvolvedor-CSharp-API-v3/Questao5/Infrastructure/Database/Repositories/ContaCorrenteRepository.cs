using Dapper;
using Microsoft.Data.Sqlite;
using Questao5.Domain.Contracts;
using Questao5.Domain.Entities;
using Questao5.Infrastructure.Exceptions;
using Questao5.Infrastructure.Sqlite;

namespace Questao5.Infrastructure.Database.Repositories;

public sealed class ContaCorrenteRepository : IContaCorrenteRepository
{
    private readonly DatabaseConfig _databaseConfig;

    public ContaCorrenteRepository(DatabaseConfig databaseConfig)
    {
        _databaseConfig = databaseConfig;
    }

    public async Task<ContaCorrente> ObterPorNumeroContaCorrenteAsync(string numeroContaCorrente)
    {
        try
        {
            ContaCorrente response = new();

            using (var connection = new SqliteConnection(_databaseConfig.Name))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@numeroContaCorrente", numeroContaCorrente, System.Data.DbType.String, System.Data.ParameterDirection.Input);

                response = await connection.QuerySingleOrDefaultAsync<ContaCorrente>(
                    $"SELECT idcontacorrente, numero, nome, ativo " +
                    $"FROM contacorrente " +
                    $"WHERE numero = @numeroContaCorrente", parameters);

                return response;
            };
        }
        catch (Exception ex)
        {
            throw new DomainException(new DomainExceptionBody(ex.Message));
        }
    }
}