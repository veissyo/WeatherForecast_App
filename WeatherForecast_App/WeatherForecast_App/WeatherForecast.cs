using System.Net.Http;
using System.Text.Json;
using System.Globalization;

namespace WeatherForecast_App;

class WeatherForecastAPICALL
{
    private string GetWeatherUrl()
    {
        return $"https://api.open-meteo.com/v1/forecast?latitude={(GetLocation.lalt ?? 0).ToString(System.Globalization.CultureInfo.InvariantCulture)}&longitude={(GetLocation.longt ?? 0).ToString(System.Globalization.CultureInfo.InvariantCulture)}&hourly=temperature_2m,rain,apparent_temperature,snowfall&current=temperature_2m,rain,snowfall,cloud_cover";
    }
    public async Task Get_Weather_Forecast()
    {
        var http = new HttpClient(); // request sender
        string dynamicUrl = GetWeatherUrl();
        var response = await http.GetAsync(dynamicUrl);
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        WeatherForecastData weatherForecastData = JsonSerializer.Deserialize<WeatherForecastData>(content);
        
        Console.WriteLine("=== Aktualna pogoda ===");
        if (weatherForecastData != null && weatherForecastData.current != null)
        {
            Console.WriteLine($"Temperatura: {weatherForecastData.current.temperature_2m}°C");
            Console.WriteLine($"Deszcz: {weatherForecastData.current.rain} mm");
            Console.WriteLine($"Śnieg: {weatherForecastData.current.snowfall} cm");
            Console.WriteLine($"Zachmurzenie: {weatherForecastData.current.cloud_cover} %");
        }

        if (weatherForecastData != null && weatherForecastData.hourly != null)
        {
            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine($"Godzina: {weatherForecastData.hourly.time[i]}");
                Console.WriteLine($"Temperatura: {weatherForecastData.hourly.temperature_2m[i]}°C");
                Console.WriteLine($"Deszcz: {weatherForecastData.hourly.rain[i]} mm");
                Console.WriteLine($"Śnieg: {weatherForecastData.hourly.snowfall[i]} cm");
                Console.WriteLine($"Odczuwana temperatura: {weatherForecastData.hourly.apparent_temperature[i]} °C");
            }
        }
    }
    
}