namespace Questao2.ServicosExternos.FootballMatches;

public interface IFootballMatches
{
    Task<int> GetTotalScoredGoalsAsync(string team, int year);
}