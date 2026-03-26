using System;
using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using Weather_App.Services;
using System.Threading.Tasks;
using Location = Weather_App.Models.Location;
namespace Weather_App.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    private readonly GeocodingService _geoService = new();
    private readonly WeatherService _weaService = new();
    [ObservableProperty] private string cityInput = "";
    [ObservableProperty] private List<Location> searchResult = new();
    [ObservableProperty] private Location? selectedCity ;
    [ObservableProperty] private double currentTemperature = 0;
    [ObservableProperty] private string statusMessage = "Enter city";

    [RelayCommand]
    private async Task SearchCityAsync()
    {
        if (string.IsNullOrWhiteSpace(CityInput))
        {
            StatusMessage = "Enter city name";
            return;
        }

        StatusMessage = "Search..";
        try
        {
            var results = await _geoService.SearchAsync(CityInput);
            SearchResult = results;
            if (SearchResult.Count == 1)
            {
                SelectedCity = SearchResult[0];
                await LoadWeatherForCityAsync();
            }
            else if (SearchResult.Count > 1)
            {
                StatusMessage = $"Найдено {SearchResult.Count} городов. Выберите один.";
            }
            else
            {
                StatusMessage = "Not Found";
            }
        }
        catch(Exception ex)
        {
            StatusMessage = $"{ex.Message}";
        }
    }

    [RelayCommand]
    private async Task LoadWeatherForCityAsync()
    {
        if (SelectedCity == null) return;
        StatusMessage = $"Load weather for {SelectedCity.Name}";
        try
        {
            var weather = await _weaService.GetWeatherAsync(SelectedCity.Latitude, SelectedCity.Longitude);
            if (weather?.Hourly?.Temperature2m?.Count == 0 || weather?.Hourly?.Temperature2m == null)
            {
                return;
            }

            var temps = weather.Hourly.Temperature2m;
            var times = weather.Hourly.Time;
            int currentIndex = GetClosestHourIndex(times);
            double currentTemp = temps[currentIndex];
            double maxTemp = temps.Max();
            CurrentTemperature = currentTemp;
            StatusMessage = $"{CurrentTemperature}C";
        }
        catch (Exception ex)
        {
            StatusMessage = $"{ex.Message}";
        }
    }

    private int GetClosestHourIndex(List<string> timeString)
    {
        if (timeString == null || timeString.Count == 0) return 0;
        DateTime now = DateTime.Now;
        DateTime bestTime = DateTime.MaxValue;
        int bestIndex = 0;
        for (int i = 0; i < timeString.Count; i++)
        {
            if (DateTime.TryParse(timeString[i], out DateTime forecastTime))
            {
                
                TimeSpan difference = (forecastTime - now).Duration(); 

                if (difference < (bestTime - now).Duration())
                {
                    bestTime = forecastTime;
                    bestIndex = i;
                }
            }
        }

        return bestIndex;
    }

    partial void OnSelectedCityChanged(Location? value)
    {
        if (value != null)
        {
            LoadWeatherForCityAsync();
        }
    }
}