namespace WeatherForecast_App;

public class APIDataProvider : IDataProvider
{
    private APIClient _client;

    public APIDataProvider()
    {
        _client = APIClient.GetInstance();
    }

    public async Task<WeatherData> GetWeatherData(LocationData location, WeatherServiceType serviceType)
    {
        var url = BuildURL(location, serviceType);

        switch (serviceType)
        {
            case WeatherServiceType.CURRENT:
                var response = await _client.GetAsync<CurrentWeatherResponse>(url);
                if (response?.current != null)
                {
                    response.current.latitude = response.latitude;
                    response.current.longitude = response.longitude;
                    response.current.timezone = response.timezone;
                    return response.current;
                }
                throw new Exception("Failed to fetch current weather data");
                
            default:
                throw new NotImplementedException($"Service type {serviceType} not implemented yet");
        }
    }

    private string BuildURL(LocationData location, WeatherServiceType serviceType)
    {
        switch (serviceType)
        {
            case WeatherServiceType.CURRENT:
                var url = $"{OpenMeteoEndpoints.FORECAST_API_URL}" +
                          $"?latitude={location.latitude.ToString(System.Globalization.CultureInfo.InvariantCulture)}" +
                          $"&longitude={location.longitude.ToString(System.Globalization.CultureInfo.InvariantCulture)}" +
                          $"&current=temperature_2m,apparent_temperature,rain,snowfall,weather_code,wind_speed_10m,cloud_cover" +
                          $"&timezone=auto";
                return url;
            
            default:
                throw new NotImplementedException($"URL building for {serviceType} not implemented yet");
        }
    }
}