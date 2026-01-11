namespace WeatherForecast_App;

public class CurrentWeatherService : IWeatherService
{
    private readonly IDataProvider _provider; // provider for the data (API or cache)
    public CurrentWeatherService(IDataProvider provider)
    {
        _provider = provider;
    }
    public async Task<WeatherData> FetchWeather(LocationData location)
    {
        var weatherData = await _provider.GetWeatherData(location, WeatherServiceType.CURRENT); // asks the provider for the data
        return weatherData; // returns the data as WeatherData
    }
}