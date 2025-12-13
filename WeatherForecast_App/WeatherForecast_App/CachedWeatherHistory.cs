namespace WeatherForecast_App;

public class CachedWeatherHistory
{
    private readonly Dictionary<string, List<WeatherHistoryMemento>> _history;

    public CachedWeatherHistory()
    {
        _history = new Dictionary<string, List<WeatherHistoryMemento>>();
    }
    
    public void SaveState(WeatherData data, LocationData location)
    {
        var key = GetLocationKey(location);
        
        if (!_history.ContainsKey(key))
        {
            _history[key] = new List<WeatherHistoryMemento>();
        }
        var memento = new WeatherHistoryMemento(data, location);
        _history[key].Add(memento);
        
        CleanOldData(key);
    }
    
    public WeatherHistoryMemento? GetLast(LocationData location)
    {
        var key = GetLocationKey(location);
        
        if (_history.ContainsKey(key) && _history[key].Count > 0)
        {
            return _history[key].Last();
        }

        return null;
    }
    
    public List<WeatherHistoryMemento> GetAll(LocationData location)
    {
        var key = GetLocationKey(location);
        return _history.ContainsKey(key) ? _history[key] : new List<WeatherHistoryMemento>();
    }
    
    private string GetLocationKey(LocationData location)
    {
        return $"{location.latitude:F2}_{location.longitude:F2}";
    }
    
    private void CleanOldData(string key)
    {
        var cutoffTime = DateTime.Now.AddHours(-24);
        _history[key] = _history[key].Where(m => m.Timestamp > cutoffTime).ToList();
    }
}