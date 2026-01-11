using System.Text.Json;

namespace WeatherForecast_App;

public class APIClient
{
    private static APIClient? _instance;
    private readonly HttpClient _httpClient;

    private APIClient() // private constructor to prevent instantiation from outside
    {
        _httpClient = new HttpClient();
    }

    public static APIClient GetInstance()
    {
        if (_instance == null)
        {
            _instance = new APIClient(); // new instance ONLY if null
        }
        return _instance;
    }
    
    public async Task<T> GetAsync<T>(string url)
    {
        try
        {
            var response = await _httpClient.GetAsync(url); // sends the request
            response.EnsureSuccessStatusCode();
            
            var json = await response.Content.ReadAsStringAsync(); // reads the response body
            return JsonSerializer.Deserialize<T>(json); // deserializes the response body 
            // it is deserialized into a WeatherTypeResponse object (latitude, longitude, timezone and the data 'wrapped')
        }
        catch (HttpRequestException ex)
        {
            throw new Exception($"API request failed: {ex.Message}", ex);
        }
    }
}