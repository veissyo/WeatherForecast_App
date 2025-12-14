namespace WeatherForecast_App;

public class GeoLocationService()
{
    private readonly APIClient _client = APIClient.GetInstance();
    
    public async Task<LocationData> GetLocation(string cityName)
    {
        var url = $"{OpenMeteoEndpoints.GEOCODING_API_URL}?name={Uri.EscapeDataString(cityName)}&count=1&language=en&format=json";
        var response = await _client.GetAsync<GeocodingResponse>(url);
        return response.results.FirstOrDefault();
    }
}