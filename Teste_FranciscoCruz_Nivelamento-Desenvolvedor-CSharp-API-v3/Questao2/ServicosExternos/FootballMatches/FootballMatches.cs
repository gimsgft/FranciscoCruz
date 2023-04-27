using Questao2.ServicosExternos.FootballMatches.Response;

namespace Questao2.ServicosExternos.FootballMatches;

public class FootballMatches : IFootballMatches
{
    private static HttpClient client = new HttpClient();
    private const string URL = "https://jsonmock.hackerrank.com/";

    public async Task<int> GetTotalScoredGoalsAsync(string team, int year)
    {
        int totalPages = 1;
        int totalScoredGoals = 0;

        for (int page = 1; page <= totalPages; page++)
        {
            var response = await RequestAsync(team, year, page);
            totalPages = response.total_paginas;
            totalScoredGoals += response.data.Sum(g => Convert.ToInt32(g.time1Gols));
        }

        return totalScoredGoals;
    }

    private async Task<FootballMatchesHeaderResponse> RequestAsync(string team, int year, int page)
    {
        var response = new FootballMatchesHeaderResponse();

        HttpResponseMessage requestResponse = await client.GetAsync($"{URL}api/football_matches?year={year}&team1={team}&page={page}");

        if (requestResponse.IsSuccessStatusCode)
        {
            string responseRequestBody = await requestResponse.Content.ReadAsStringAsync();
            var responseRequest = Newtonsoft.Json.JsonConvert.DeserializeObject<FootballMatchesHeaderResponse>(responseRequestBody);

            response = responseRequest ?? response;
        }

        return response;
    }
}
