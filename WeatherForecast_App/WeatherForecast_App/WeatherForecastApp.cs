namespace WeatherForecast_App;

public class WeatherForecastApp
{
private readonly GeoLocationService _geoService;
    private readonly IDataProvider _dataProvider;
    private readonly WeatherServiceFactory _serviceFactory;
    private readonly Dictionary<string, WatchedLocation> _watchedLocations;
    private readonly WeatherAnalyzer _analyzer;
    private readonly JSONDataExporter _exporter;
    private readonly JSONDataImporter _importer;
    private WeatherData? _lastWeatherData;
    private string? _lastCityName;
    private WeatherReport? _lastReport;

    public WeatherForecastApp()
    {
        _geoService = new GeoLocationService();
        
        var apiProvider = new APIDataProvider();
        _dataProvider = new CachedDataProvider(apiProvider, cacheValidityMinutes: 30);
        
        _serviceFactory = new WeatherServiceFactory(_dataProvider);
        _watchedLocations = new Dictionary<string, WatchedLocation>();
        
        _analyzer = new WeatherAnalyzer();
        _exporter = new JSONDataExporter();
        _importer = new JSONDataImporter();
    }

    public void ShowMenu()
    {
        Console.WriteLine("\n" + new string('=', 70));
        Console.WriteLine("                  WEATHER FORECAST APP                 ");
        Console.WriteLine(new string('=', 70));
        Console.WriteLine("  WEATHER:");
        Console.WriteLine("    [1] Current weather");
        Console.WriteLine("    [2] 7-day forecast");
        Console.WriteLine("    [3] Hourly forecast (24h)");
        Console.WriteLine("    [4] Weather history (from last month)");
        Console.WriteLine();
        Console.WriteLine("  ALERTS AND OBSERVED LOCATIONS:");
        Console.WriteLine("    [5] Add location to observed");
        Console.WriteLine("    [6] Add an alert to observed location");
        Console.WriteLine("    [7] Update all observed location alert data");
        Console.WriteLine("    [8] Show observed locations");
        Console.WriteLine("    [9] Delete observed location");
        Console.WriteLine();
        Console.WriteLine("  ANALYSIS:");
        Console.WriteLine("    [10] Precipitation analysis");
        Console.WriteLine("    [11] Temperature trend analysis");
        Console.WriteLine();
        Console.WriteLine("  REPORTS:");
        Console.WriteLine("    [12] Location comparison report");
        Console.WriteLine("    [13] Weekly alerts report");
        Console.WriteLine();
        Console.WriteLine("  EXPORT/IMPORT (JSON):");
        Console.WriteLine("    [14] Export latest weather data");
        Console.WriteLine("    [15] Export watched locations");
        Console.WriteLine("    [16] Export latest report");
        Console.WriteLine("    [17] Import weather data");
        Console.WriteLine("    [18] Import watched locations");
        Console.WriteLine();
        Console.WriteLine("    [0] Exit");
        Console.WriteLine(new string('=', 70));
    }

    public async Task<CurrentWeatherData?> GetWeatherForCity(string cityName)
    {
        try
        {
            var location = await _geoService.GetLocation(cityName);
            if (location == null)
            {
                Console.WriteLine($"Location not found: {cityName}");
                return null;
            }

            var service = _serviceFactory.CreateWeatherService(WeatherServiceType.CURRENT);
            var data = await service.FetchWeather(location);
            
            _lastWeatherData = data;
            _lastCityName = cityName;
            
            return data as CurrentWeatherData;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error when fetching weather: {ex.Message}");
            return null;
        }
    }

    public async Task<DailyWeatherData?> GetForecast(string cityName, int days)
    {
        try
        {
            var location = await _geoService.GetLocation(cityName);
            if (location == null)
            {
                Console.WriteLine($"Location not found: {cityName}");
                return null;
            }

            var service = _serviceFactory.CreateWeatherService(WeatherServiceType.DAILY_7);
            var data = await service.FetchWeather(location);
            
            _lastWeatherData = data;
            _lastCityName = cityName;
            
            return data as DailyWeatherData;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Weather fetch error: {ex.Message}");
            return null;
        }
    }

    public async Task<HourlyWeatherData?> GetHourlyForecast(string cityName)
    {
        try
        {
            var location = await _geoService.GetLocation(cityName);
            if (location == null)
            {
                Console.WriteLine($"Location not found: {cityName}");
                return null;
            }

            var service = _serviceFactory.CreateWeatherService(WeatherServiceType.HOURLY_24);
            var data = await service.FetchWeather(location);
            
            _lastWeatherData = data;
            _lastCityName = cityName;
            
            return data as HourlyWeatherData;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Weather fetch error: {ex.Message}");
            return null;
        }
    }

    public async Task<HistoricalWeatherData?> GetHistoricalWeather(string cityName)
    {
        try
        {
            var location = await _geoService.GetLocation(cityName);
            if (location == null)
            {
                Console.WriteLine($"Location not found: {cityName}");
                return null;
            }

            var service = _serviceFactory.CreateWeatherService(WeatherServiceType.HISTORICAL);
            var data = await service.FetchWeather(location);
            
            return data as HistoricalWeatherData;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Weather fetch error: {ex.Message}");
            return null;
        }
    }

    public async Task<WatchedLocation?> AddWatchedLocation(string cityName)
    {
        try
        {
            if (_watchedLocations.ContainsKey(cityName))
            {
                Console.WriteLine($"Location {cityName} is already being observed.");
                return _watchedLocations[cityName];
            }

            var location = await _geoService.GetLocation(cityName);
            if (location == null)
            {
                Console.WriteLine($"Location not found: {cityName}");
                return null;
            }

            var watched = new WatchedLocation(location);
            _watchedLocations[cityName] = watched;
            
            Console.WriteLine($"Added to observed locations: {cityName}");
            return watched;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error adding location to observed: {ex.Message}");
            return null;
        }
    }

    public async Task AddAlertToLocation(string cityName, WeatherAlert alert)
    {
        if (!_watchedLocations.ContainsKey(cityName))
        {
            Console.WriteLine($"Location {cityName} isn't being observed.");
            return;
        }

        var watched = _watchedLocations[cityName];
        watched.Attach(alert);
        Console.WriteLine($"Added alert to location: {cityName}");
    }

    public async Task UpdateAllWatchedLocations()
    {
        if (_watchedLocations.Count == 0)
        {
            Console.WriteLine("No watched locations.");
            return;
        }

        Console.WriteLine($"\nUpdating {_watchedLocations.Count} observed locations...\n");

        foreach (var kvp in _watchedLocations)
        {
            var cityName = kvp.Key;
            var watched = kvp.Value;

            try
            {
                var currentService = _serviceFactory.CreateWeatherService(WeatherServiceType.CURRENT);
                var currentData = await currentService.FetchWeather(watched._locationData);

                var dailyService = _serviceFactory.CreateWeatherService(WeatherServiceType.DAILY_7);
                var dailyData = await dailyService.FetchWeather(watched._locationData);

                watched.UpdateWeather(
                    currentData as CurrentWeatherData, 
                    dailyData as DailyWeatherData
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating {cityName}: {ex.Message}");
            }
        }
    }

    public void ShowWatchedLocations()
    {
        if (_watchedLocations.Count == 0)
        {
            Console.WriteLine("\nNo watched locations.");
            return;
        }

        Console.WriteLine("\n" + new string('=', 70));
        Console.WriteLine("WATCHED LOCATIONS:");
        Console.WriteLine(new string('=', 70));

        foreach (var kvp in _watchedLocations)
        {
            var cityName = kvp.Key;
            var watched = kvp.Value;
            var alerts = watched.GetAlerts();

            Console.WriteLine($"\nLocalization: {cityName}");
            Console.WriteLine($"  Coordinates: ({watched._locationData.latitude:F2}, {watched._locationData.longitude:F2})");
            Console.WriteLine($"  Alert count: {alerts.Count}");
            
            if (alerts.Count > 0)
            {
                Console.WriteLine("  Active alerts:");
                foreach (var alert in alerts)
                {
                    Console.WriteLine($"    - {alert.GetType().Name.Replace("Alert", "")}");
                }
            }
        }
        Console.WriteLine(new string('=', 70));
    }

    public void RemoveWatchedLocation(string cityName)
    {
        if (_watchedLocations.Remove(cityName))
        {
            Console.WriteLine($"Deleted location: {cityName}");
        }
        else
        {
            Console.WriteLine($"Location {cityName} isn't watched.");
        }
    }

    public async Task PerformPrecipitationAnalysis(string cityName)
    {
        try
        {
            var forecast = await GetForecast(cityName, 7);
            if (forecast == null) return;

            _analyzer.SetStrategy(new Precipitation());
            var result = _analyzer.PerformAnalysis(forecast);

            Console.WriteLine("\n" + new string('=', 70));
            Console.WriteLine($"PRECIPITATION ANALYSIS: {cityName}");
            Console.WriteLine(new string('=', 70));
            Console.WriteLine(result);
            Console.WriteLine(new string('=', 70));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"analysis error: {ex.Message}");
        }
    }

    public async Task PerformTemperatureTrendAnalysis(string cityName)
    {
        try
        {
            var forecast = await GetForecast(cityName, 7);
            if (forecast == null) return;

            _analyzer.SetStrategy(new TemperatureTrend());
            var result = _analyzer.PerformAnalysis(forecast);

            Console.WriteLine("\n" + new string('=', 70));
            Console.WriteLine($"TEMPERATURE TREND ANALYSIS: {cityName}");
            Console.WriteLine(new string('=', 70));
            Console.WriteLine(result);
            Console.WriteLine(new string('=', 70));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"analysis error: {ex.Message}");
        }
    }

    public async Task GenerateLocationComparisonReport(string city1, string city2)
    {
        try
        {
            var location1 = await _geoService.GetLocation(city1);
            var location2 = await _geoService.GetLocation(city2);

            if (location1 == null || location2 == null)
            {
                Console.WriteLine("Location not found.");
                return;
            }

            var forecast1 = await GetForecast(city1, 7);
            var forecast2 = await GetForecast(city2, 7);

            if (forecast1 == null || forecast2 == null)
            {
                Console.WriteLine("Weather fetch error.");
                return;
            }

            var builder = new WeatherReportBuilder(ReportType.LOCATION_COMPARISON);
            var director = new ReportDirector(builder);

            var report = director.BuildLocationComparisonReport(
                location1, location2, forecast1, forecast2, city1, city2
            );
            
            _lastReport = report;

            Console.WriteLine(report.ToString());
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Raport error: {ex.Message}");
        }
    }

    public async Task GenerateWeeklyAlertsReport(string cityName)
    {
        try
        {
            if (!_watchedLocations.ContainsKey(cityName))
            {
                Console.WriteLine($"Location {cityName} isn't watched.");
                Console.WriteLine("Add it and configure alerts.");
                return;
            }

            var watched = _watchedLocations[cityName];
            var forecast = await GetForecast(cityName, 7);

            if (forecast == null)
            {
                Console.WriteLine("Weather fetch error.");
                return;
            }

            var alerts = watched.GetAlerts();
            var builder = new WeatherReportBuilder(ReportType.WEEKLY_ALERTS);
            var director = new ReportDirector(builder);

            var report = director.BuildWeeklyAlertsReport(
                watched._locationData, forecast, cityName, alerts
            );
            
            _lastReport = report;

            Console.WriteLine(report.ToString());
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Raport error: {ex.Message}");
        }
    }

    public List<string> GetWatchedLocationNames()
    {
        return _watchedLocations.Keys.ToList();
    }
     public async Task ExportLastWeatherData()
    {
        if (_lastWeatherData == null || _lastCityName == null)
        {
            Console.WriteLine("No data to export. First fetch weather data.");
            return;
        }

        var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
        var fileName = $"weather_{_lastCityName}_{timestamp}.json";
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), fileName);

        await _exporter.ExportWeatherData(_lastWeatherData, filePath, _lastCityName);
    }

    public async Task ExportWatchedLocations()
    {
        if (_watchedLocations.Count == 0)
        {
            Console.WriteLine("No watched locations to export. First add some.");
            return;
        }

        var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
        var fileName = $"watched_locations_{timestamp}.json";
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), fileName);

        await _exporter.ExportWatchedLocations(_watchedLocations, filePath);
    }

    public async Task ExportLastReport()
    {
        if (_lastReport == null)
        {
            Console.WriteLine("No reports to export. First generate one.");
            return;
        }

        var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
        var fileName = $"report_{timestamp}.json";
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), fileName);

        await _exporter.ExportWeatherReport(_lastReport, filePath);
    }

    public async Task ImportWeatherData(string filePath)
    {
        var data = await _importer.ImportWeatherData(filePath);

        if (data != null)
        {
            Console.WriteLine("\nImported data:");
            Console.WriteLine(data.GetSummary());
        }
    }

    public async Task ImportWatchedLocations(string filePath)
    {
        var locations = await _importer.ImportWatchedLocations(filePath);

        if (locations != null && locations.Count > 0)
        {
            foreach (var kvp in locations)
            {
                if (!_watchedLocations.ContainsKey(kvp.Key))
                {
                    var watched = new WatchedLocation(kvp.Value);
                    _watchedLocations[kvp.Key] = watched;
                    Console.WriteLine($"Added an observed location: {kvp.Key}");
                }
                else
                {
                    Console.WriteLine($"Location {kvp.Key} already added to observed.");
                }
            }
        }
    }
}