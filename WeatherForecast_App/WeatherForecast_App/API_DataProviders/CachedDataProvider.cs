namespace WeatherForecast_App;

public class CachedDataProvider : IDataProvider
{
    private readonly CachedWeatherHistory _cache;
    private readonly IDataProvider _fallbackProvider;
    private readonly int _cacheValidityMinutes;

    public CachedDataProvider(IDataProvider fallbackProvider, int cacheValidityMinutes = 30)
    {
        _cache = new CachedWeatherHistory();
        _fallbackProvider = fallbackProvider;
        _cacheValidityMinutes = cacheValidityMinutes;
    }

    public async Task<WeatherData> GetWeatherData(LocationData location, WeatherServiceType serviceType)
    {
        var cachedData = _cache.GetLast(location, serviceType);
        
        if (cachedData != null)
        {
            var age = DateTime.Now - cachedData.Timestamp;

            if (age.TotalMinutes < _cacheValidityMinutes)
            {
                Console.WriteLine($"Cache data loaded. (Age: {age.TotalMinutes:F1} min)");
                return cachedData.Data;
            }
        }
        Console.WriteLine("Cache data unavailable or expired. Fetching data from API.");
        var freshData = await _fallbackProvider.GetWeatherData(location, serviceType);
        _cache.SaveState(freshData, location, serviceType);
        return freshData;
    }
}