namespace WeatherForecast_App;

public interface IWeatherService
{
    public Task<WeatherData> FetchWeather(LocationData location);
}

public enum WeatherServiceType
{
    CURRENT
}