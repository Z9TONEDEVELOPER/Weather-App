using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.Json;
using System;
using System.Text.Json.Serialization;
using Location = Weather_App.Models.Location;

namespace Weather_App.Services;

public class GeocodingService
{
    private readonly HttpClient _client = new HttpClient();

    public async Task<List<Location>> SearchAsync(string cityName)
    {
        string url = $"https://geocoding-api.open-meteo.com/v1/search?name={Uri.EscapeDataString(cityName)}&count=5&language=ru&format=json";
        string json = await _client.GetStringAsync(url);
        var response = JsonSerializer.Deserialize<GeocodingResponse>(json,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        return response?.Results ?? new List<Models.Location>();
    }

    public class GeocodingResponse
    {
        [JsonPropertyName("results")] public List<Location> Results { get; set; } = new();
    }
}