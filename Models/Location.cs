using System.Text.Json.Serialization;
namespace Weather_App.Models;

public class Location
{
    [JsonPropertyName("name")] 
    public string Name { get; set; } = "";
    [JsonPropertyName("latitude")]
    public double Latitude { get; set; }
    [JsonPropertyName("longitude")]
    public double Longitude { get; set; }
    [JsonPropertyName("country")] 
    public string Country { get; set; } = "";
    [JsonPropertyName("admin1")] 
    public string? Admin1 { get; set; }
    public string DisplayName => string.IsNullOrEmpty(Admin1)?$"{Name}, {Country}":$"{Name}, {Admin1}, {Country}";

}