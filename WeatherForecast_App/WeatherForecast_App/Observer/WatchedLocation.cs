namespace WeatherForecast_App;

public class WatchedLocation : ISubject 
{
    public readonly LocationData _locationData;
    private readonly List<IWeatherObserver> _observers;
    private CurrentWeatherData? _currentWeather;
    private DailyWeatherData? _forecast;

    public WatchedLocation(LocationData location)
    {
        _locationData = location;
        _observers = new List<IWeatherObserver>();
    }

    public void Attach(IWeatherObserver observer)
    {
        if (!_observers.Contains(observer))
        {
            _observers.Add(observer);
            Console.WriteLine($"Added observer {observer}.");
        }
    }

    public void Detach(IWeatherObserver observer)
    {
        if (_observers.Remove(observer))
        {
            Console.WriteLine($"An observer has been deleted.");
        }
    }
    public void Notify()
    {
        if (_currentWeather == null) return;
        Console.WriteLine($"\nNotifying {_observers.Count} observers...");
        foreach (var observer in _observers)
        {
            observer.Update(_currentWeather);
        }
    }

    public void UpdateWeather(CurrentWeatherData current, DailyWeatherData? forecast = null)
    {
        _currentWeather = current;
        _forecast = forecast;
        
        Console.WriteLine($"Weather has been updated.");
        Notify();
    }
}