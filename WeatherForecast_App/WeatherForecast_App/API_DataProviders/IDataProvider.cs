namespace WeatherForecast_App;

public interface IDataProvider // interface needed for dependency injection/strategy
{
    public Task<WeatherData> GetWeatherData(LocationData location, WeatherServiceType serviceType);
}