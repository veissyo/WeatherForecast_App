namespace WeatherForecast_App;

public class CurrentWeatherService : IWeatherService
{
    private readonly IDataProvider _provider;
    public CurrentWeatherService(IDataProvider provider)
    {
        _provider = provider;
    }
    public async Task<WeatherData> FetchWeather(LocationData location)
    {
        var weatherData = await _provider.GetWeatherData(location, WeatherServiceType.CURRENT);
        return weatherData;
    }
}