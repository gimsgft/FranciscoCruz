using Newtonsoft.Json;

namespace Questao2.ServicosExternos.FootballMatches.Response;

public class FootballMatchesBodyResponse
{
    [JsonProperty("competition")]
    public string? competicao { get; set; }
    [JsonProperty("year")]
    public int? ano { get; set; }
    [JsonProperty("round")]
    public string? rodada { get; set; }
    [JsonProperty("team1")]
    public string? time1 { get; set; }
    [JsonProperty("team2")]
    public string? time2 { get; set; }
    [JsonProperty("team1goals")]
    public string? time1Gols { get; set; }
    [JsonProperty("team2goals")]
    public string? time2Gols { get; set; }
}