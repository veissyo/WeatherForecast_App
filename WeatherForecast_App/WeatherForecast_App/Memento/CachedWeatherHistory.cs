namespace WeatherForecast_App;

public class CachedWeatherHistory // memento's caretaker (saves/reads)
{
    private readonly Dictionary<string, List<WeatherHistoryMemento>> _history; // has a unique key for each saved weather data

    public CachedWeatherHistory()
    {
        _history = new Dictionary<string, List<WeatherHistoryMemento>>(); // can have many mementos for each location
    }
    
    public void SaveState(WeatherData data, LocationData location, WeatherServiceType serviceType)
    {
        var key = GetLocationKey(location, serviceType);
        if (!_history.ContainsKey(key))
        {
            _history[key] = new List<WeatherHistoryMemento>();
        }
        var memento = new WeatherHistoryMemento(data, location, serviceType); // creates a memento from the weather data
        _history[key].Add(memento); // adds the memento to the list
        
        CleanOldData(key);
    }
    
    public WeatherHistoryMemento? GetLast(LocationData location, WeatherServiceType serviceType) 
    // returns last saved weather data for said location and weather type
    {
        var key = GetLocationKey(location, serviceType);
        
        if (_history.ContainsKey(key) && _history[key].Count > 0)
        {
            return _history[key].Last();
        }

        return null;
    }
    
    private string GetLocationKey(LocationData location, WeatherServiceType serviceType) // makes a unique key with location and service type
    {
        return $"{location.latitude:F2}_{location.longitude:F2}_{serviceType}";
    }
    
    private void CleanOldData(string key)
    {
        var cutoffTime = DateTime.Now.AddHours(-24);
        _history[key] = _history[key].Where(m => m.Timestamp > cutoffTime).ToList();
    }
}