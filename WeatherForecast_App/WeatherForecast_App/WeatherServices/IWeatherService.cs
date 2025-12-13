namespace WeatherForecast_App;

public interface IWeatherService
{
    public Task<WeatherData> FetchWeather(LocationData location);
}

public enum WeatherServiceType
{
    CURRENT,
    DAILY_7,
    HOURLY_24,
    HISTORICAL
}