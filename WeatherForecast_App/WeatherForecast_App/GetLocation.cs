namespace WeatherForecast_App;
using System.Text.Json;
public class GetLocation
{
    private string _city;
    public static float? lalt;
    public static float? longt;
    public GetLocation()
    {
        Console.WriteLine("Wybierz miasto:");
        _city = Console.ReadLine() ?? string.Empty;
    }

    private string GetLocationUrl()
    {
        return $"https://geocoding-api.open-meteo.com/v1/search?name={_city}&count=10&language=en&format=json";
    }
    
    public async Task GetLocationName()
    {
        var http = new HttpClient(); // request sender
        var url = GetLocationUrl();
        var response = await http.GetAsync(url);
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        LocationData locationData = JsonSerializer.Deserialize<LocationData>(content);
        lalt = locationData.results[0]?.latitude ?? 0;
        longt = locationData.results[0]?.longitude ?? 0;
    }
}