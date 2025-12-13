namespace WeatherForecast_App;

public interface IDataProvider
{
    public Task<WeatherData> GetWeatherData(LocationData location, WeatherServiceType serviceType);
}