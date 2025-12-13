namespace WeatherForecast_App;

public class WatchedLocation : ISubject 
{
    private readonly LocationData _locationData;
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
            Console.WriteLine($"Dodano obserwatora do wybranego miasta.");
        }
    }

    public void Detach(IWeatherObserver observer)
    {
        if (_observers.Remove(observer))
        {
            Console.WriteLine($" Usunięto obserwatora.");
        }
    }
    public void Notify()
    {
        if (_currentWeather == null) return;
        Console.WriteLine($"\nPowiadamiam {_observers.Count} obserwatorów...");
        foreach (var observer in _observers)
        {
            observer.Update(_currentWeather);
        }
    }

    public void UpdateWeather(CurrentWeatherData current, DailyWeatherData? forecast = null)
    {
        _currentWeather = current;
        _forecast = forecast;
        
        Console.WriteLine($"Zaktualizowano pogodę!");
        Notify();
    }
}