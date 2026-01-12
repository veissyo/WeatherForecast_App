namespace WeatherForecast_App;

public class WeatherForecastApp
{
    private readonly GeoLocationService _geoService; // city name -> latitude/longitude converter
    private readonly IDataProvider _dataProvider; // data provider for weather data (cached/api)
    private readonly WeatherServiceFactory _serviceFactory; // creates weather services based on type
    private readonly Dictionary<string, WatchedLocation> _watchedLocations; // watched locations (key = city name, value = WatchedLocation (subject))
    private readonly WeatherAnalyzer _analyzer; // weather analysis strategy
    private readonly JSONDataExporter _exporter;
    private readonly JSONDataImporter _importer;
    private WeatherData? _lastWeatherData; // last fetched weather data
    private string? _lastCityName; // last fetched city name
    private WeatherReport? _lastReport; // last generated report

    public WeatherForecastApp() // initializes all services etc. service -- > constructor
    {
        _geoService = new GeoLocationService(); 
        var apiProvider = new APIDataProvider(); // local variable
        _dataProvider = new CachedDataProvider(apiProvider, cacheValidityMinutes: 30); // dependency injection
        _serviceFactory = new WeatherServiceFactory(_dataProvider); // already has the data provider
        _watchedLocations = new Dictionary<string, WatchedLocation>();
        _analyzer = new WeatherAnalyzer();
        _exporter = new JSONDataExporter();
        _importer = new JSONDataImporter();
    }

    public void ShowMenu()
    {
        Console.WriteLine("\n" + new string('-', 70));
        Console.WriteLine("                  WEATHER FORECAST APP                 ");
        Console.WriteLine(new string('-', 70));
        Console.WriteLine("  WEATHER:");
        Console.WriteLine("    [ 1 ] Current weather");
        Console.WriteLine("    [ 2 ] 7-day forecast");
        Console.WriteLine("    [ 3 ] Hourly forecast (24h)");
        Console.WriteLine("    [ 4 ] Weather history (from last month)");
        Console.WriteLine();
        Console.WriteLine("  ALERTS AND OBSERVED LOCATIONS:");
        Console.WriteLine("    [ 5 ] Add location to observed");
        Console.WriteLine("    [ 6 ] Add an alert to observed location");
        Console.WriteLine("    [ 7 ] Update all observed location alert data");
        Console.WriteLine("    [ 8 ] Show observed locations");
        Console.WriteLine("    [ 9 ] Delete observed location");
        Console.WriteLine();
        Console.WriteLine("  ANALYSIS:");
        Console.WriteLine("    [ 10 ] Precipitation analysis");
        Console.WriteLine("    [ 11 ] Temperature trend analysis");
        Console.WriteLine();
        Console.WriteLine("  REPORTS:");
        Console.WriteLine("    [ 12 ] Location comparison report");
        Console.WriteLine("    [ 13 ] Weekly alerts report");
        Console.WriteLine();
        Console.WriteLine("  EXPORT/IMPORT (JSON):");
        Console.WriteLine("    [ 14 ] Export latest weather data");
        Console.WriteLine("    [ 15 ] Export watched locations");
        Console.WriteLine("    [ 16 ] Export latest report");
        Console.WriteLine("    [ 17 ] Import weather data");
        Console.WriteLine("    [ 18 ] Import watched locations");
        Console.WriteLine();
        Console.WriteLine("    [ 0 ] Exit");
        Console.WriteLine(new string('-', 70));
    }

    public async Task<CurrentWeatherData?> GetWeatherForCity(string cityName) // promise / null
    {
        try
        {
            var location = await _geoService.GetLocation(cityName); // sends API requests, gets LocationData
            
            if (location == null)
            {
                Console.WriteLine($"Location not found: {cityName}");
                return null;
            }

            // data provider for the service is being given in constructor
            var service = _serviceFactory.CreateWeatherService(WeatherServiceType.CURRENT); // creates a service based on type
            var data = await service.FetchWeather(location); // fetches weather data through service
            
            _lastWeatherData = data; // saves the last data for export 
            _lastCityName = cityName; // -"-
            
            return data as CurrentWeatherData; // cast WeatherData to CurrentWeatherData (safe cast)
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error when fetching weather: {ex.Message}");
            return null;
        }
    }

    public async Task<DailyWeatherData?> GetForecast(string cityName)
    {
        try
        {
            var location = await _geoService.GetLocation(cityName); // gets LocationData from GeoLocationService
            
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
            Console.WriteLine($"Error when fetching weather: {ex.Message}");
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
            Console.WriteLine($"Error when fetching weather: {ex.Message}");
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
            Console.WriteLine($"Error when fetching weather: {ex.Message}");
            return null;
        }
    }

    public async Task<WatchedLocation?> AddWatchedLocation(string cityName) // adds a place to observed locations
    {
        try
        {
            if (_watchedLocations.ContainsKey(cityName)) // checks if the place is already being observed
            {
                Console.WriteLine($"Location {cityName} is already being observed.");
                return _watchedLocations[cityName];
            }

            var location = await _geoService.GetLocation(cityName); // gets LocationData from GeoLocationService
            if (location == null)
            {
                Console.WriteLine($"Location not found: {cityName}");
                return null;
            }

            var watched = new WatchedLocation(location); // new WatchedLocation (subject of the observer pattern)
            _watchedLocations[cityName] = watched; // adds the location 
            
            Console.WriteLine($"Added to observed locations: {cityName}");
            return watched;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error adding location to observed: {ex.Message}");
            return null;
        }
    }

    public async Task AddAlertToLocation(string cityName, WeatherAlert alert) // dictionary key, WeatherAlert object (observer)
    {
        if (!_watchedLocations.ContainsKey(cityName)) // checks if the place is being observed
        {
            Console.WriteLine($"Location {cityName} isn't being observed.");
            return;
        }

        var watched = _watchedLocations[cityName];
        watched.Attach(alert); // adds an alert to the location
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

        foreach (var kvp in _watchedLocations) // key value pairs, updates weather for each location
        {
            var cityName = kvp.Key;
            var watched = kvp.Value;

            try
            {
                var hourlyService = _serviceFactory.CreateWeatherService(WeatherServiceType.HOURLY_24);
                var hourlyData = await hourlyService.FetchWeather(watched._locationData); // fetches weather data through service
                watched.UpdateWeather(hourlyData as HourlyWeatherData); // updates the watched location with fetched data
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

        Console.WriteLine("\n" + new string('-', 70));
        Console.WriteLine("WATCHED LOCATIONS:");
        Console.WriteLine(new string('-', 70));

        foreach (var kvp in _watchedLocations) // prints the city name, coordinates and it's active alerts
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
        Console.WriteLine(new string('-', 70));
    }

    public void RemoveWatchedLocation(string cityName)
    {
        if (_watchedLocations.Remove(cityName)) // removes the location from the dictionary
        {
            Console.WriteLine($"Deleted location: {cityName}.");
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
            var forecast = await GetForecast(cityName); // gets forecast data from the API
            if (forecast == null) return;

            _analyzer.SetStrategy(new Precipitation()); // sets the strategy for the analysis
            var result = _analyzer.PerformAnalysis(forecast); // analyzer calls the strategy's Analyze method

            Console.WriteLine("\n" + new string('-', 70));
            Console.WriteLine($"PRECIPITATION ANALYSIS: {cityName}");
            Console.WriteLine(new string('-', 70));
            Console.WriteLine(result);
            Console.WriteLine(new string('-', 70));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Analysis error: {ex.Message}");
        }
    }

    public async Task PerformTemperatureTrendAnalysis(string cityName)
    {
        try
        {
            var forecast = await GetForecast(cityName); // gets forecast data from the API
            if (forecast == null) return;

            _analyzer.SetStrategy(new TemperatureTrend()); // sets the strategy for the analysis
            var result = _analyzer.PerformAnalysis(forecast); // analyzer calls the strategy's Analyze method

            Console.WriteLine("\n" + new string('-', 70));
            Console.WriteLine($"TEMPERATURE TREND ANALYSIS: {cityName}");
            Console.WriteLine(new string('-', 70));
            Console.WriteLine(result);
            Console.WriteLine(new string('-', 70));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Analysis error: {ex.Message}");
        }
    }

    public async Task GenerateLocationComparisonReport(string city1, string city2)
    {
        try
        {
            var location1 = await _geoService.GetLocation(city1); // gets LocationData from GeoLocationService for city 1 and 2
            var location2 = await _geoService.GetLocation(city2);

            if (location1 == null || location2 == null)
            {
                Console.WriteLine("Location(s) not found.");
                return;
            }

            var forecast1 = await GetForecast(city1); // fetches forecast (7 days ofc) for both cities
            var forecast2 = await GetForecast(city2);

            if (forecast1 == null || forecast2 == null)
            {
                Console.WriteLine("Weather fetch error.");
                return;
            }

            var builder = new WeatherReportBuilder(ReportType.LOCATION_COMPARISON); // makes a builder of a specific type
            var director = new ReportDirector(builder); // gives the director the builder
            var report = director.BuildLocationComparisonReport(location1, location2, forecast1, forecast2, city1, city2);
            _lastReport = report; // saves the report for export
            
            Console.WriteLine(report.ToString());
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Report error: {ex.Message}");
        }
    }

    public async Task GenerateWeeklyAlertsReport(string cityName)
    {
        try
        {
            if (!_watchedLocations.ContainsKey(cityName)) // only observed locations can generate reports
            {
                Console.WriteLine($"Location {cityName} isn't watched.");
                Console.WriteLine("Add it and configure alerts.");
                return;
            }

            var watched = _watchedLocations[cityName]; // gets the watched location from the dictionary
            var forecast = await GetForecast(cityName); // gets forecast data

            if (forecast == null)
            {
                Console.WriteLine("Weather fetch error.");
                return;
            }

            var alerts = watched.GetAlerts(); // fetches alerts from the watched location
            var builder = new WeatherReportBuilder(ReportType.WEEKLY_ALERTS); // creates a builder of a specific type
            var director = new ReportDirector(builder); // gives the director the builder
            var report = director.BuildWeeklyAlertsReport(watched._locationData, forecast, cityName, alerts);
            _lastReport = report; // saves the report for export
            
            Console.WriteLine(report.ToString());
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Report error: {ex.Message}");
        }
    }

    public List<string> GetWatchedLocationNames() // returns a list of city names for Main to use
    {
        return _watchedLocations.Keys.ToList();
    }
    
     public async Task ExportLastWeatherData() // exporting the last fetched weather data
    {
        if (_lastWeatherData == null || _lastCityName == null)
        {
            Console.WriteLine("No data to export. First fetch weather data.");
            return;
        }

        var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
        var fileName = $"weather_{_lastCityName}_{timestamp}.json"; // makes a file name based on the city name and timestamp
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), fileName); // gets the full path

        await _exporter.ExportWeatherData(_lastWeatherData, filePath, _lastCityName); // exports the data, delegating it to the exporter
    }

    public async Task ExportWatchedLocations() // exporting the watched locations
    {
        if (_watchedLocations.Count == 0)
        {
            Console.WriteLine("No watched locations to export. First add some.");
            return;
        }

        var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
        var fileName = $"watched_locations_{timestamp}.json"; // makes a file name based on the timestamp
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), fileName); // gets the full path

        await _exporter.ExportWatchedLocations(_watchedLocations, filePath); // exports the data, delegating it to the exporter
    }

    public async Task ExportLastReport() // exporting the last generated report
    {
        if (_lastReport == null)
        {
            Console.WriteLine("No reports to export. First generate one.");
            return;
        }

        var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
        var fileName = $"report_{timestamp}.json"; // makes a file name based on the timestamp
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), fileName); // gets the full path

        await _exporter.ExportWeatherReport(_lastReport, filePath); // exports the data, delegating it to the exporter
    }

    public async Task ImportWeatherData(string filePath) // imports weather data from a JSON file
    {
        var data = await _importer.ImportWeatherData(filePath); // imports the data, delegating it to the importer

        if (data != null)
        {
            Console.WriteLine("\nImported data:");
            Console.WriteLine(data.GetSummary());
        }
    }

    public async Task ImportWatchedLocations(string filePath) // imports watched locations from a JSON file
    {
        var locations = await _importer.ImportWatchedLocations(filePath); // imports the data, delegating it to the importer

        if (locations != null && locations.Count > 0)
        {
            foreach (var kvp in locations) // adds the imported locations to the dictionary
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