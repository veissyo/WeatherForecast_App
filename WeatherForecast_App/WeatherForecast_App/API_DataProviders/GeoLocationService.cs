namespace WeatherForecast_App;

public class GeoLocationService()
{
    private readonly APIClient _client = APIClient.GetInstance();
    
    public async Task<LocationData> GetLocation(string cityName)
    {
        // builds the url
        var url = $"{OpenMeteoEndpoints.GEOCODING_API_URL}?name={Uri.EscapeDataString(cityName)}&count=1&language=en&format=json";
        // asks the API client to send a request with said url and waits for it's response (deserialized, API CLient does it)
        var response = await _client.GetAsync<GeocodingResponse>(url);
        // returns the first result (LocationData)
        return response.results.FirstOrDefault();
    }
}