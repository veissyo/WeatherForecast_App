namespace WeatherForecast_App;

public class HistoricalWeatherService : IWeatherService
{
    private readonly IDataProvider _provider;
    private string[] _startDate;
    private string[] _endDate;
    
    public HistoricalWeatherService(IDataProvider provider)
    {
        _provider = provider;
    }

    public void SetDateRange(string[] start, string[] end)
    {
        _startDate = start;
        _endDate = end;
    }

    public async Task<WeatherData> FetchWeather(LocationData location)
    {
        var weatherData = await _provider.GetWeatherData(location, WeatherServiceType.HISTORICAL);
        return weatherData;
    }
}