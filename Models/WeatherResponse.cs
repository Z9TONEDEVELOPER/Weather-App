using System.Collections.Generic;
using System.Text.Json.Serialization;
namespace Weather_App.Models;

public class WeatherResponse
{
    [JsonPropertyName("hourly")] public HourlyData Hourly { get; set; } = new();
}

public class HourlyData
{
    [JsonPropertyName("time")] public List<string> Time { get; set; } = new();
    [JsonPropertyName("temperature_2m")] public List<double> Temperature2m { get; set; } = new();
    [JsonPropertyName("weather_code")] public List<int> WeatherCode { get; set; } = new();
}