namespace WeatherForecast_App;

public interface ISubject
{
    void Attach(IWeatherObserver observer);
    void Detach(IWeatherObserver observer);
    void Notify();
}