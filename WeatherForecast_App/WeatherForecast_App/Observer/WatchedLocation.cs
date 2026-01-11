namespace WeatherForecast_App;

public class WatchedLocation : ISubject 
{
    public readonly LocationData _locationData;
    private readonly List<IWeatherObserver> _observers; // we can have multiple observers
    private HourlyWeatherData? _hourlyForecast;

    public WatchedLocation(LocationData location)
    {
        _locationData = location;
        _observers = new List<IWeatherObserver>();
    }

    public void Attach(IWeatherObserver observer) // registers an observer
    {
        if (!_observers.Contains(observer))
        {
            _observers.Add(observer);
        }
    }

    // the detach method has not been implemented, as we don't need it. we remove the whole watched location, not just one observer.
    
    public void Notify()
    {
        if (_hourlyForecast == null) return;
        //Console.WriteLine($"\nNotifying {_observers.Count} observers...");
        foreach (var observer in _observers) // notify all observers
        {
            observer.Update(_hourlyForecast); // update each observer (WeatherAlert)
        }
    }

    public void UpdateWeather(HourlyWeatherData forecast)
    {
        _hourlyForecast = forecast;
        Console.WriteLine($"Weather has been updated.");
        Notify();
    }
    
    public List<WeatherAlert> GetAlerts()
    {
        return _observers.OfType<WeatherAlert>().ToList();
    }
}