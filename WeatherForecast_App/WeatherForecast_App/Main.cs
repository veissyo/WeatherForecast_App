using System.Text.Json;

namespace WeatherForecast_App;

class Program
{
    public static async Task Main()
    {
        var app = new WeatherForecastApp();

        Console.WriteLine("Welcome to weather forecast app!\n!");

        while (true)
        {
            app.ShowMenu();
            Console.Write("\nPick an option: ");
            var choice = Console.ReadLine();

            try
            {
                switch (choice)
                {
                    case "1":
                        await HandleCurrentWeather(app);
                        break;

                    case "2":
                        await HandleDailyForecast(app);
                        break;

                    case "3":
                        await HandleHourlyForecast(app);
                        break;

                    case "4":
                        await HandleHistoricalWeather(app);
                        break;

                    case "5":
                        await HandleAddWatchedLocation(app);
                        break;

                    case "6":
                        await HandleAddAlert(app);
                        break;

                    case "7":
                        await app.UpdateAllWatchedLocations();
                        break;

                    case "8":
                        app.ShowWatchedLocations();
                        break;

                    case "9":
                        await HandleRemoveWatchedLocation(app);
                        break;

                    case "10":
                        await HandlePrecipitationAnalysis(app);
                        break;

                    case "11":
                        await HandleTemperatureTrendAnalysis(app);
                        break;

                    case "12":
                        await HandleLocationComparisonReport(app);
                        break;

                    case "13":
                        await HandleWeeklyAlertsReport(app);
                        break;
                    
                    case "14":
                        await HandleExportWeatherData(app);
                        break;

                    case "15":
                        await HandleExportWatchedLocations(app);
                        break;

                    case "16":
                        await HandleExportReport(app);
                        break;

                    case "17":
                        await HandleImportWeatherData(app);
                        break;

                    case "18":
                        await HandleImportWatchedLocations(app);
                        break;

                    case "0":
                        Console.WriteLine("\nBye! :3");
                        return;

                    default:
                        Console.WriteLine("\nNo such option");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nerror: {ex.Message}");
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
            Console.Clear();
        }
    }

    private static async Task HandleCurrentWeather(WeatherForecastApp app)
    {
        Console.Write("\nPlease select a city: ");
        var city = Console.ReadLine();
        
        if (string.IsNullOrWhiteSpace(city))
        {
            Console.WriteLine("No such name");
            return;
        }

        var weather = await app.GetWeatherForCity(city);
        if (weather != null)
        {
            Console.WriteLine("\n" + new string('=', 70));
            Console.WriteLine($"Current weather: {city}");
            Console.WriteLine(new string('=', 70));
            Console.WriteLine(weather.GetSummary());
            Console.WriteLine($"Time: {weather.GetFormattedTime()}");
            Console.WriteLine($"Place: ({weather.latitude:F2}, {weather.longitude:F2})");
            Console.WriteLine($"Time zone: {weather.timezone}");
            Console.WriteLine(new string('=', 70));
        }
    }

    private static async Task HandleDailyForecast(WeatherForecastApp app)
    {
        Console.Write("\nPlease select a city: ");
        var city = Console.ReadLine();
        
        if (string.IsNullOrWhiteSpace(city))
        {
            Console.WriteLine("No such name.");
            return;
        }

        var forecast = await app.GetForecast(city);
        if (forecast != null)
        {
            Console.WriteLine("\n" + new string('=', 70));
            Console.WriteLine($"Forecast for 7 days: {city}");
            Console.WriteLine(new string('=', 70));
            Console.WriteLine(forecast.GetSummary());
            Console.WriteLine($"Place: ({forecast.latitude:F2}, {forecast.longitude:F2})");
            Console.WriteLine($"Time zone: {forecast.timezone}");
            Console.WriteLine(new string('=', 70));
        }
    }

    private static async Task HandleHourlyForecast(WeatherForecastApp app)
    {
        Console.Write("\nPlease select a city: ");
        var city = Console.ReadLine();
        
        if (string.IsNullOrWhiteSpace(city))
        {
            Console.WriteLine("No such name.");
            return;
        }

        var hourly = await app.GetHourlyForecast(city);
        if (hourly != null)
        {
            Console.WriteLine("\n" + new string('=', 70));
            Console.WriteLine($"Hourly forecast (24h): {city}");
            Console.WriteLine(new string('=', 70));
            Console.WriteLine(hourly.GetSummary());
            Console.WriteLine($"Location: ({hourly.latitude:F2}, {hourly.longitude:F2})");
            Console.WriteLine($"Time zone: {hourly.timezone}");
            Console.WriteLine(new string('=', 70));
        }
    }

    private static async Task HandleHistoricalWeather(WeatherForecastApp app)
    {
        Console.Write("\nPlease select a city: ");
        var city = Console.ReadLine();
        
        if (string.IsNullOrWhiteSpace(city))
        {
            Console.WriteLine("No such name.");
            return;
        }

        var historical = await app.GetHistoricalWeather(city);
        if (historical != null)
        {
            Console.WriteLine("\n" + new string('=', 70));
            Console.WriteLine($"Weather history from the last 30 days: {city}");
            Console.WriteLine(new string('=', 70));
            
            if (historical.daily != null)
            {
                Console.WriteLine(historical.daily.GetSummary());
                Console.WriteLine($"Location: ({historical.latitude:F2}, {historical.longitude:F2})");
                Console.WriteLine($"Time zone: {historical.timezone}");
            }
            else
            {
                Console.WriteLine("No weather history registered.");
            }
            
            Console.WriteLine(new string('=', 70));
        }
    }

    private static async Task HandleAddWatchedLocation(WeatherForecastApp app)
    {
        Console.Write("\nPlease select a city: ");
        var city = Console.ReadLine();
        
        if (string.IsNullOrWhiteSpace(city))
        {
            Console.WriteLine("No such name.");
            return;
        }

        var watched = await app.AddWatchedLocation(city);
        
        if (watched != null)
        {
            Console.WriteLine($"\nLocation {city} has been added to the list of observed locations.");
            Console.WriteLine("You can now add alerts to this location (option 6).");
        }
    }

    private static async Task HandleAddAlert(WeatherForecastApp app)
    {
        var watchedLocations = app.GetWatchedLocationNames();
        
        if (watchedLocations.Count == 0)
        {
            Console.WriteLine("\nNo observed locations. Observe some locations (option 5) first, then try adding an alert.");
            return;
        }

        Console.WriteLine("\nObserved locations:");
        for (int i = 0; i < watchedLocations.Count; i++)
        {
            Console.WriteLine($"  [{i + 1}] {watchedLocations[i]}");
        }

        Console.Write("\nPick the location to add an alert to: ");
        if (!int.TryParse(Console.ReadLine(), out int locationChoice) || 
            locationChoice < 1 || locationChoice > watchedLocations.Count)
        {
            Console.WriteLine("No such choice.");
            return;
        }

        var cityName = watchedLocations[locationChoice - 1];

        Console.WriteLine("\nAviable alert types:");
        Console.WriteLine("  [1] Temperature alert");
        Console.WriteLine("  [2] Thunderstorm alert");
        Console.WriteLine("  [3] Flood alert");
        Console.WriteLine("  [4] Fog alert");
        Console.WriteLine("  [5] Snow alert");
        
        Console.Write("\nPick the alert type: ");
        var alertChoice = Console.ReadLine();

        WeatherAlert? alert = null;

        switch (alertChoice)
        {
            case "1":
                Console.Write("Temperature threshold (C): ");
                if (double.TryParse(Console.ReadLine(), out var temp))
                {
                    Console.Write("Alert for temperature under or over the threshold? (o/u): ");
                    var above = Console.ReadLine()?.ToLower() == "o";
                    alert = new TemperatureAlert(cityName, temp, above);
                    Console.WriteLine($"A temperature alert has been added: {(above ? "over" : "under")} {temp}C");
                }
                else
                {
                    Console.WriteLine("No such value.");
                }
                break;

            case "2":
                alert = new ThunderstormAlert(cityName);
                Console.WriteLine("Thunderstorm alert added.");
                break;

            case "3":
                alert = new FloodAlert(cityName);
                Console.WriteLine("Flood alert added.");
                break;

            case "4":
                alert = new FogAlert(cityName);
                Console.WriteLine("Fog alert added.");
                break;

            case "5":
                alert = new SnowAlert(cityName);
                Console.WriteLine("Snow alert added.");
                break;

            default:
                Console.WriteLine("Wrong choice.");
                return;
        }

        if (alert != null)
        {
            await app.AddAlertToLocation(cityName, alert);
        }
    }

    private static async Task HandleRemoveWatchedLocation(WeatherForecastApp app)
    {
        var watchedLocations = app.GetWatchedLocationNames();
        
        if (watchedLocations.Count == 0)
        {
            Console.WriteLine("\nNo observed locations.");
            return;
        }

        Console.WriteLine("\nObserved locations:");
        for (int i = 0; i < watchedLocations.Count; i++)
        {
            Console.WriteLine($"  [{i + 1}] {watchedLocations[i]}");
        }

        Console.Write("\nPick an observed location to remove: ");
        if (!int.TryParse(Console.ReadLine(), out int choice) || 
            choice < 1 || choice > watchedLocations.Count)
        {
            Console.WriteLine("No such choice.");
            return;
        }

        var cityName = watchedLocations[choice - 1];
        app.RemoveWatchedLocation(cityName);
    }

    private static async Task HandlePrecipitationAnalysis(WeatherForecastApp app)
    {
        Console.Write("\nPlease select a city: ");
        var city = Console.ReadLine();
        
        if (string.IsNullOrWhiteSpace(city))
        {
            Console.WriteLine("No such name.");
            return;
        }

        await app.PerformPrecipitationAnalysis(city);
    }

    private static async Task HandleTemperatureTrendAnalysis(WeatherForecastApp app)
    {
        Console.Write("\nPlease select a city: ");
        var city = Console.ReadLine();
        
        if (string.IsNullOrWhiteSpace(city))
        {
            Console.WriteLine("No such name.");
            return;
        }

        await app.PerformTemperatureTrendAnalysis(city);
    }

    private static async Task HandleLocationComparisonReport(WeatherForecastApp app)
    {
        Console.Write("\nPlease select the first city: ");
        var city1 = Console.ReadLine();
        
        Console.Write("Please select the second city: ");
        var city2 = Console.ReadLine();
        
        if (string.IsNullOrWhiteSpace(city1) || string.IsNullOrWhiteSpace(city2))
        {
            Console.WriteLine("No such names.");
            return;
        }

        await app.GenerateLocationComparisonReport(city1, city2);
    }

    private static async Task HandleWeeklyAlertsReport(WeatherForecastApp app)
    {
        var watchedLocations = app.GetWatchedLocationNames();
        
        if (watchedLocations.Count == 0)
        {
            Console.WriteLine("\nNo observed locations.");
            Console.WriteLine("First, add an observed location and configure alerts.");
            return;
        }

        Console.WriteLine("\nObserved locations:");
        for (int i = 0; i < watchedLocations.Count; i++)
        {
            Console.WriteLine($"  [{i + 1}] {watchedLocations[i]}");
        }

        Console.Write("\nPick the location to generate the report for: ");
        if (!int.TryParse(Console.ReadLine(), out int choice) || 
            choice < 1 || choice > watchedLocations.Count)
        {
            Console.WriteLine("No such choice.");
            return;
        }

        var cityName = watchedLocations[choice - 1];
        await app.GenerateWeeklyAlertsReport(cityName);
    }
    private static async Task HandleExportWeatherData(WeatherForecastApp app)
    {
        await app.ExportLastWeatherData();
    }

    private static async Task HandleExportWatchedLocations(WeatherForecastApp app)
    {
        await app.ExportWatchedLocations();
    }

    private static async Task HandleExportReport(WeatherForecastApp app)
    {
        await app.ExportLastReport();
    }

    private static async Task HandleImportWeatherData(WeatherForecastApp app)
    {
        Console.Write("\nPlease add the path to the file to import:");
        var filePath = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(filePath))
        {
            Console.WriteLine("Wrong path.");
            return;
        }

        await app.ImportWeatherData(filePath);
    }

    private static async Task HandleImportWatchedLocations(WeatherForecastApp app)
    {
        Console.Write("\nPlease add the path to the file to import:");
        var filePath = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(filePath))
        {
            Console.WriteLine("Wrong path.");
            return;
        }

        await app.ImportWatchedLocations(filePath);
    }
}
