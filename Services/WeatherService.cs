
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.Json;
using System;
using System.Text.Json.Serialization;
using Avalonia.Controls;
using Weather_App.Models;

namespace Weather_App.Services;

public class WeatherService
{
    private readonly HttpClient _client = new HttpClient();

    public async Task<WeatherResponse?> GetWeatherAsync(double lat, double lon)
    {
        string url = $"https://api.open-meteo.com/v1/forecast?" +
                     $"latitude={lat.ToString(System.Globalization.CultureInfo.InvariantCulture)}" +
                     $"&longitude={lon.ToString(System.Globalization.CultureInfo.InvariantCulture)}" +
                     $"&hourly=temperature_2m" +
                     $"&timezone=Europe%2FMoscow" +
                     $"&forecast_days=1";
        string json = await _client.GetStringAsync(url);
        return JsonSerializer.Deserialize<WeatherResponse>(json,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }
}