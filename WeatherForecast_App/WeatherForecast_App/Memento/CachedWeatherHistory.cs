namespace WeatherForecast_App;

public class CachedWeatherHistory
{
    private readonly Dictionary<string, List<WeatherHistoryMemento>> _history;

    public CachedWeatherHistory()
    {
        _history = new Dictionary<string, List<WeatherHistoryMemento>>();
    }
    
    public void SaveState(WeatherData data, LocationData location, WeatherServiceType serviceType)
    {
        var key = GetLocationKey(location, serviceType);
        if (!_history.ContainsKey(key))
        {
            _history[key] = new List<WeatherHistoryMemento>();
        }
        var memento = new WeatherHistoryMemento(data, location);
        _history[key].Add(memento);
        
        CleanOldData(key);
    }
    
    public WeatherHistoryMemento? GetLast(LocationData location, WeatherServiceType serviceType)
    {
        var key = GetLocationKey(location, serviceType);
        
        if (_history.ContainsKey(key) && _history[key].Count > 0)
        {
            return _history[key].Last();
        }

        return null;
    }
    
    private string GetLocationKey(LocationData location, WeatherServiceType serviceType)
    {
        return $"{location.latitude:F2}_{location.longitude:F2}_{serviceType}";
    }
    
    private void CleanOldData(string key)
    {
        var cutoffTime = DateTime.Now.AddHours(-24);
        _history[key] = _history[key].Where(m => m.Timestamp > cutoffTime).ToList();
    }
}