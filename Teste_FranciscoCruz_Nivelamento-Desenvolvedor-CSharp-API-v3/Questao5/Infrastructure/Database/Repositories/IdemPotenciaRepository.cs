using Dapper;
using Microsoft.Data.Sqlite;
using Questao5.Domain.Contracts;
using Questao5.Infrastructure.Exceptions;
using Questao5.Infrastructure.Sqlite;

namespace Questao5.Infrastructure.Database.Repositories;

public sealed class IdemPotenciaRepository : IIdemPotenciaRepository
{
    private readonly DatabaseConfig _databaseConfig;

    public IdemPotenciaRepository(DatabaseConfig databaseConfig)
    {
        _databaseConfig = databaseConfig;
    }

    public async Task CriarAsync(string chave_Idempotencia)
    {
        try
        {
            using var connection = new SqliteConnection(_databaseConfig.Name);

            await connection.ExecuteAsync(
                $"INSERT INTO idemPotencia(chave_idempotencia, requisicao, resultado) " +
                $"VALUES('{chave_Idempotencia}', '', '');");
        }
        catch (Exception ex)
        {
            throw new DomainException(new DomainExceptionBody(ex.Message));
        }
    }

    public async Task<bool> JaExisteAsync(string chaveIdempotencia)
    {
        try
        {
            int quantidade = 0;
            using (var connection = new SqliteConnection(_databaseConfig.Name))
            {

                var parameters = new DynamicParameters();
                parameters.Add("@chaveIdempotencia", chaveIdempotencia, System.Data.DbType.Int32, System.Data.ParameterDirection.Input);

                quantidade = await connection.QuerySingleOrDefaultAsync<int>("SELECT 1 FROM idempotencia WHERE [chave_idempotencia] = @chaveIdempotencia", parameters);
            }

            return quantidade > 0;
        }
        catch (Exception ex)
        {
            throw new DomainException(new DomainExceptionBody(ex.Message));
        }
    }
}