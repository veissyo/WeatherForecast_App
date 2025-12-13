namespace WeatherForecast_App;

public class WeatherForecastApp
{
    private readonly WeatherServiceFactory _factory;
    private readonly GeoLocationService _geoService;
    private readonly List<WatchedLocation> _watchedLocations;
    private readonly ReportDirector _reportDirector;
    private readonly CachedDataProvider _cachedProvider;

    public WeatherForecastApp()
    {
        var apiProvider = new APIDataProvider();
        _cachedProvider = new CachedDataProvider(apiProvider, cacheValidityMinutes: 30);
        _factory = new WeatherServiceFactory(_cachedProvider);
        _geoService = new GeoLocationService();
        _watchedLocations = new List<WatchedLocation>();
        var builder = new WeatherReportBuilder(ReportType.CURRENT_CONDITIONS);
        _reportDirector = new ReportDirector(builder);

    }

    public async Task<CurrentWeatherData> GetWeatherForCity(string cityName)
    {
        try
        {
            var location = await _geoService.GetLocation(cityName);
            if (location == null)
            {
                Console.WriteLine($"Nie znaleziono miasta: {cityName}");
                return null;
            }

            var service = _factory.CreateWeatherService(WeatherServiceType.CURRENT);
            var data = await service.FetchWeather(location);
            
            return data as CurrentWeatherData;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Błąd: {ex.Message}");
            return null;
        }
    }

    public async Task<DailyWeatherData> GetForecast(string cityName, int days = 7)
    {
        try
        {
            var location = await _geoService.GetLocation(cityName);
            if (location == null)
            {
                Console.WriteLine($"Nie znaleziono miasta: {cityName}");
                return null;
            }

            var service = _factory.CreateWeatherService(WeatherServiceType.DAILY_7);
            var data = await service.FetchWeather(location);
            
            return data as DailyWeatherData;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Błąd: {ex.Message}");
            return null;
        }
    }
    
    public async Task<HourlyWeatherData> GetHourlyForecast(string cityName)
    {
        try
        {
            var location = await _geoService.GetLocation(cityName);
            if (location == null)
            {
                Console.WriteLine($"Nie znaleziono miasta: {cityName}");
                return null;
            }

            var service = _factory.CreateWeatherService(WeatherServiceType.HOURLY_24);
            var data = await service.FetchWeather(location);
            
            return data as HourlyWeatherData;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Błąd: {ex.Message}");
            return null;
        }
    }

    // ✅ DODANE - Dane historyczne
    public async Task<HistoricalWeatherData> GetHistoricalWeather(string cityName)
    {
        try
        {
            var location = await _geoService.GetLocation(cityName);
            if (location == null)
            {
                Console.WriteLine($"Nie znaleziono miasta: {cityName}");
                return null;
            }

            var service = _factory.CreateWeatherService(WeatherServiceType.HISTORICAL);
            var data = await service.FetchWeather(location);
            
            return data as HistoricalWeatherData;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Błąd: {ex.Message}");
            return null;
        }
    }
    public async Task<WatchedLocation?> AddWatchedLocation(string cityName)
    {
        try
        {
            var location = await _geoService.GetLocation(cityName);
            if (location == null)
            {
                Console.WriteLine($"Nie znaleziono miasta: {cityName}");
                return null;
            }

            var watched = new WatchedLocation(location);
            _watchedLocations.Add(watched);
            
            Console.WriteLine($"Dodano obserwowaną lokalizację: {cityName}");
            return watched;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Błąd: {ex.Message}");
            return null;
        }
    }

    public async Task AddAlertToLocation(string cityName, WeatherAlert alert)
    {
        var watched = _watchedLocations.FirstOrDefault(w => 
            w._locationData.latitude.ToString().Contains(cityName) || 
            w._locationData.longitude.ToString().Contains(cityName));

        if (watched == null)
        {
            watched = await AddWatchedLocation(cityName);
        }

        if (watched != null)
        {
            watched.Attach(alert);
            Console.WriteLine($"Dodano alert do lokalizacji");
        }
    }

    public async Task UpdateAllWatchedLocations()
    {
        Console.WriteLine($"\nAktualizuję {_watchedLocations.Count} obserwowanych lokalizacji...");
        
        foreach (var watched in _watchedLocations)
        {
            try
            {
                var currentService = _factory.CreateWeatherService(WeatherServiceType.CURRENT);
                var forecastService = _factory.CreateWeatherService(WeatherServiceType.DAILY_7);
                
                var current = await currentService.FetchWeather(watched._locationData) as CurrentWeatherData;
                var forecast = await forecastService.FetchWeather(watched._locationData) as DailyWeatherData;
                
                if (current != null)
                {
                    watched.UpdateWeather(current, forecast);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✗ Błąd aktualizacji: {ex.Message}");
            }
        }
    }

    // Tworzy raport
    public async Task<WeatherReport?> CreateReport(ReportType type, string cityName)
    {
        try
        {
            var location = await _geoService.GetLocation(cityName);
            if (location == null) return null;

            switch (type)
            {
                case ReportType.CURRENT_CONDITIONS:
                    var current = await GetWeatherForCity(cityName);
                    return current != null ? _reportDirector.BuildCurrentConditionsReport(location, current) : null;

                case ReportType.WEEKLY_FORECAST:
                    var forecast = await GetForecast(cityName, 7);
                    return forecast != null ? _reportDirector.BuildWeeklyForecastReport(location, forecast) : null;
                case ReportType.HOURLY_FORECAST:
                    var hourly = await GetHourlyForecast(cityName);
                    return hourly != null ? _reportDirector.BuildHourlyForecastReport(location, hourly) : null;

                case ReportType.HISTORICAL_ANALYSIS:
                    var historical = await GetHistoricalWeather(cityName);
                    return historical != null ? _reportDirector.BuildHistoricalReport(location, historical) : null;
                default:
                    Console.WriteLine($"✗ Nieobsługiwany typ raportu: {type}");
                    return null;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"✗ Błąd tworzenia raportu: {ex.Message}");
            return null;
        }
    }

    // Porównuje lokalizacje
    /*public async Task<ComparisonResult?> CompareLocations(List<string> cityNames)
    {
        try
        {
            var locations = new List<LocationData>();
            
            foreach (var city in cityNames)
            {
                var loc = await _geoService.GetLocation(city);
                if (loc != null) locations.Add(loc);
            }

            if (locations.Count < 2)
            {
                Console.WriteLine("✗ Potrzeba przynajmniej 2 lokalizacji do porównania");
                return null;
            }

            return await _analysisEngine.CompareLocations(locations);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"✗ Błąd porównania: {ex.Message}");
            return null;
        }
    }*/
/*
    // Eksportuje dane
    public bool ExportData(string filepath, string format = "json")
    {
        var data = new ExportableData
        {
            Locations = _watchedLocations,
            ExportDate = DateTime.Now
        };

        return _exportManager.Export(data, filepath, format);
    }

    // Importuje dane
    public bool ImportData(string filepath)
    {
        var data = _exportManager.Import(filepath);
        return data != null;
    }
*/
    // Wyświetla menu
    public void ShowMenu()
    {
        Console.WriteLine("\n" + new string('=', 60));
        Console.WriteLine("          🌦️  WEATHER APPLICATION - MENU  🌦️");
        Console.WriteLine(new string('=', 60));
        Console.WriteLine("1. Sprawdź aktualną pogodę");
        Console.WriteLine("2. Prognoza na 7 dni");
        Console.WriteLine("3. Prognoza godzinowa (24h)");        
        Console.WriteLine("4. Historia pogody (ostatni miesiąc)"); 
        Console.WriteLine("5. Dodaj obserwowaną lokalizację + alert");
        Console.WriteLine("6. Aktualizuj obserwowane lokalizacje");
        Console.WriteLine("7. Stwórz raport");
        Console.WriteLine("0. Wyjście");
    }
}