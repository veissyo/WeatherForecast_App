using System.Text.Json;

namespace WeatherForecast_App;

public class APIClient
{
    private static APIClient? _instance;
    private readonly HttpClient _httpClient;

    private APIClient()
    {
        _httpClient = new HttpClient();
    }

    public static APIClient GetInstance()
    {
        if (_instance == null)
        {
            _instance = new APIClient();
        }
        return _instance;
    }
    
    public async Task<T?> GetAsync<T>(string url)
    {
        try
        {
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(json);
        }
        catch (HttpRequestException ex)
        {
            throw new Exception($"API request failed: {ex.Message}", ex);
        }
    }
}