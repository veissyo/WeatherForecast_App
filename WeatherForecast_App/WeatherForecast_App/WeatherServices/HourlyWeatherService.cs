namespace WeatherForecast_App;

public class HourlyWeatherService : IWeatherService
{
    private readonly IDataProvider _provider;

    public HourlyWeatherService(IDataProvider provider)
    {
        _provider = provider;
    }
    
    public async Task<WeatherData> FetchWeather(LocationData location) // gets weather data from API or cache (hence task)
    {
        var weatherData = await _provider.GetWeatherData(location, WeatherServiceType.HOURLY_24);
        return weatherData; 
    }
}