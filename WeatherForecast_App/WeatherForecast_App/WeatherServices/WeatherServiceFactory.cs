namespace WeatherForecast_App;

public class WeatherServiceFactory
{
    private readonly IDataProvider _provider;

    public WeatherServiceFactory(IDataProvider provider)
    {
        _provider = provider;
    }
    
    public IWeatherService CreateWeatherService(WeatherServiceType type)
    {
        switch (type)
        {
            case WeatherServiceType.CURRENT:
                return new CurrentWeatherService(_provider);
                
            case WeatherServiceType.DAILY_7:
                return new DailyWeatherService(_provider);
                
            case WeatherServiceType.HOURLY_24:
                return new HourlyWeatherService(_provider);
                
            case WeatherServiceType.HISTORICAL:
                return new HistoricalWeatherService(_provider);
                
            default:
                throw new ArgumentException($"Unknown service type: {type}");
        }
    }
}