using Newtonsoft.Json;

namespace Questao2.ServicosExternos.FootballMatches.Response;

public class FootballMatchesHeaderResponse
{
    [JsonProperty("page")]
    public int pagina { get; set; }
    [JsonProperty("per_page")]
    public int por_pagina { get; set; }
    [JsonProperty("total")]
    public int total { get; set; }
    [JsonProperty("total_pages")]
    public int total_paginas { get; set; }
    [JsonProperty("data")]
    public List<FootballMatchesBodyResponse> data { get; set; } = new List<FootballMatchesBodyResponse>();
}