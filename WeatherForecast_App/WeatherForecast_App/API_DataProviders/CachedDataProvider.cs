namespace WeatherForecast_App;

public class CachedDataProvider : IDataProvider
{
    private readonly CachedWeatherHistory _cache;
    private readonly IDataProvider _fallbackProvider; // API provider
    private readonly int _cacheValidityMinutes;

    public CachedDataProvider(IDataProvider fallbackProvider, int cacheValidityMinutes = 30) 
    {
        _cache = new CachedWeatherHistory();
        _fallbackProvider = fallbackProvider;
        _cacheValidityMinutes = cacheValidityMinutes;
    }

    public async Task<WeatherData> GetWeatherData(LocationData location, WeatherServiceType serviceType)
    {
        var cachedData = _cache.GetLast(location, serviceType); // checks if there's data in the cache
        // using location + service type as a key
        
        if (cachedData != null) 
        {
            var age = DateTime.Now - cachedData.Timestamp; // checks the age of the data

            if (age.TotalMinutes < _cacheValidityMinutes) // if it's not expired, return the cached data
            {
                Console.WriteLine($"Cache data loaded. (Age: {age.TotalMinutes:F1} min)");
                return cachedData.Data;
            }
        }
        Console.WriteLine("Cache data unavailable or expired. Fetching data from API.");
        var freshData = await _fallbackProvider.GetWeatherData(location, serviceType); // falls back to the API provider and fetches data there
        _cache.SaveState(freshData, location, serviceType); // if there was no saved data, save it to cache
        return freshData;
    }
}