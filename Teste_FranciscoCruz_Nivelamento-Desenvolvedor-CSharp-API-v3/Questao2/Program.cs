using Questao2.ServicosExternos.FootballMatches;

public class Program
{
    public static async Task Main()
    {
        IFootballMatches footballMatches = new FootballMatches();
        string teamName = "Paris Saint-Germain";
        int year = 2013;
        int totalGoals = await footballMatches.GetTotalScoredGoalsAsync(teamName, year);        

        Console.WriteLine("Team "+ teamName +" scored "+ totalGoals.ToString() + " goals in "+ year);

        teamName = "Chelsea";
        year = 2014;
        totalGoals = await footballMatches.GetTotalScoredGoalsAsync(teamName, year);

        Console.WriteLine("Team " + teamName + " scored " + totalGoals.ToString() + " goals in " + year);

        // Output expected:
        // Team Paris Saint - Germain scored 109 goals in 2013
        // Team Chelsea scored 92 goals in 2014
    }
}