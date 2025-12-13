namespace WeatherForecast_App;

public interface IWeatherObserver
{
    void Update(WeatherData data);
}