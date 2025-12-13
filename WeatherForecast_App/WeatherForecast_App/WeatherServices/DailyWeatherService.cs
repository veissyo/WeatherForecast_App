namespace WeatherForecast_App;

public class DailyWeatherService : IWeatherService
{
    private readonly IDataProvider _provider;

    public DailyWeatherService(IDataProvider provider)
    {
        _provider = provider;
    }
    
    public async Task<WeatherData> FetchWeather(LocationData location)
    {
        var weatherData = await _provider.GetWeatherData(location, WeatherServiceType.DAILY_7);
        return weatherData;
    }
}