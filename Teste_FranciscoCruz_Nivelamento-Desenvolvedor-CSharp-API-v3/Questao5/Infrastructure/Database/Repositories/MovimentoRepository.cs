using Dapper;
using Microsoft.Data.Sqlite;
using Questao5.Domain.Contracts;
using Questao5.Domain.Entities;
using Questao5.Infrastructure.Exceptions;
using Questao5.Infrastructure.Sqlite;

namespace Questao5.Infrastructure.Database.Repositories;

public sealed class MovimentoRepository : IMovimentoRepository
{
    private readonly DatabaseConfig _databaseConfig;

    public MovimentoRepository(DatabaseConfig databaseConfig)
    {
        _databaseConfig = databaseConfig;
    }

    public async Task<string> CriarAsync(Movimento movimento)
    {
        try
        {
            int ultimoId = 0;
            using var connection = new SqliteConnection(_databaseConfig.Name);

            ultimoId = await connection.QuerySingleOrDefaultAsync<int>(
                $"INSERT INTO movimento(idmovimento, idcontacorrente, datamovimento, tipomovimento, valor) " +
                $"VALUES('{Guid.NewGuid()}', '{movimento.IdContaCorrente}', '{DateTime.Now}', '{movimento.TipoMovimento}', '{movimento.Valor}');" +
                $"select last_insert_rowid();");

            return ultimoId.ToString();
        }
        catch (Exception ex)
        {
            throw new DomainException(new DomainExceptionBody(ex.Message));
        }
    }

    public async Task<List<Movimento>> ObterMovimentosAsync(string idContaCorrente)
    {
        try
        {
            List<Movimento> response = new();

            using (var connection = new SqliteConnection(_databaseConfig.Name))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@idContaCorrente", idContaCorrente, System.Data.DbType.String, System.Data.ParameterDirection.Input);

                response = (List<Movimento>)await connection.QueryAsync<Movimento>(
                    $"SELECT mov.idmovimento, mov.idcontacorrente, mov.datamovimento, mov.tipomovimento, REPLACE(mov.valor, ',', '.') AS valor " +
                    $"FROM movimento mov " +
                    $"INNER JOIN contacorrente cc ON cc.idcontacorrente = mov.idcontacorrente " +
                    $"WHERE cc.numero = @idContaCorrente", parameters);

                return response;
            };
        }
        catch (Exception ex)
        {
            throw new DomainException(new DomainExceptionBody(ex.Message));
        }
    }
}