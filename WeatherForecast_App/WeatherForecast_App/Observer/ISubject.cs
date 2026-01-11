namespace WeatherForecast_App;

public interface ISubject
{
    void Attach(IWeatherObserver observer);
    void Notify();
}