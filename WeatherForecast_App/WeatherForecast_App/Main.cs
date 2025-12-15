using System.Text.Json;

namespace WeatherForecast_App;

class Program
{
    public static async Task Main()
    {
        var app = new WeatherForecastApp();

        while (true)
        {
            app.ShowMenu();
            Console.Write("\nWybierz opcję: ");
            var choice = Console.ReadLine();

            try
            {
                switch (choice)
                {
                    case "1": // Aktualna pogoda
                        Console.Write("Podaj nazwę miasta: ");
                        var city1 = Console.ReadLine();
                        if (!string.IsNullOrWhiteSpace(city1))
                        {
                            var weather = await app.GetWeatherForCity(city1);
                            if (weather != null)
                            {
                                Console.WriteLine("\n" + new string('=', 60));
                                Console.WriteLine($"☀️ :3  AKTUALNA POGODA: {city1}");
                                Console.WriteLine(new string('=', 60));
                                Console.WriteLine(weather.GetSummary());
                                Console.WriteLine($"Czas: {weather.time}");
                                Console.WriteLine(new string('=', 60));
                            }
                        }

                        break;

                    case "2": // Prognoza 7 dni
                        Console.Write("Podaj nazwę miasta: ");
                        var city2 = Console.ReadLine();
                        if (!string.IsNullOrWhiteSpace(city2))
                        {
                            var forecast = await app.GetForecast(city2, 7);
                            if (forecast != null)
                            {
                                Console.WriteLine("\n" + new string('=', 60));
                                Console.WriteLine($"📅  PROGNOZA NA 7 DNI: {city2}");
                                Console.WriteLine(new string('=', 60));
                                Console.WriteLine(forecast.GetSummary());
                                Console.WriteLine(new string('=', 60));
                            }
                        }

                        break;

                    case "3":
                        Console.Write("Podaj nazwę miasta: ");
                        var city3 = Console.ReadLine();
                        if (!string.IsNullOrWhiteSpace(city3))
                        {
                            var hourly = await app.GetHourlyForecast(city3);
                            if (hourly != null)
                            {
                                Console.WriteLine("\n" + new string('=', 60));
                                Console.WriteLine($"🕐  PROGNOZA GODZINOWA (24h): {city3}");
                                Console.WriteLine(new string('=', 60));
                                Console.WriteLine(hourly.GetSummary());
                                Console.WriteLine(new string('=', 60));
                            }
                        }
                        break;

                    // ✅ DODANE - Historia pogody
                    case "4":
                        Console.Write("Podaj nazwę miasta: ");
                        var city4 = Console.ReadLine();
                        if (!string.IsNullOrWhiteSpace(city4))
                        {
                            var historical = await app.GetHistoricalWeather(city4);
                            if (historical != null)
                            {
                                Console.WriteLine("\n" + new string('=', 60));
                                Console.WriteLine($"📊  HISTORIA POGODY: {city4}");
                                Console.WriteLine(new string('=', 60));
                                Console.WriteLine(historical.GetSummary());
                                Console.WriteLine(new string('=', 60));
                            }
                        }
                        break;

                    case "5": // Dodaj obserwowaną + alert
                        Console.Write("Podaj nazwę miasta: ");
                        var city5 = Console.ReadLine();
                        if (!string.IsNullOrWhiteSpace(city5))
                        {
                            var watched = await app.AddWatchedLocation(city5);
                            if (watched != null)
                            {
                                Console.WriteLine("\nJaki alert dodać?");
                                Console.WriteLine("1. Alert temperatury");
                                Console.WriteLine("2. Alert burzy");
                                Console.WriteLine("3. Alert powodzi");
                                Console.Write("Wybór: ");
                                var alertChoice = Console.ReadLine();

                                WeatherAlert? alert = null;
                                switch (alertChoice)
                                {
                                    case "1":
                                        Console.Write("Próg temperatury (°C): ");
                                        if (double.TryParse(Console.ReadLine(), out var temp))
                                        {
                                            Console.Write("Powyżej progu? (t/n): ");
                                            var above = Console.ReadLine()?.ToLower() == "t";
                                            alert = new TemperatureAlert(city5, temp, above);
                                        }
                                        break;
                                    case "2":
                                        alert = new ThunderstormAlert(city5);
                                        break;
                                    case "3":
                                        alert = new FloodAlert(city5);
                                        break;
                                }

                                if (alert != null)
                                {
                                    watched.Attach(alert);
                                }
                            }
                        }
                        break;

                    case "6": // Aktualizuj obserwowane
                        await app.UpdateAllWatchedLocations();
                        break;

                    case "7": // Stwórz raport
                        Console.Write("Podaj nazwę miasta: ");
                        var city7 = Console.ReadLine();
                        if (!string.IsNullOrWhiteSpace(city7))
                        {
                            Console.WriteLine("\nTyp raportu:");
                            Console.WriteLine("1. Aktualne warunki");
                            Console.WriteLine("2. Prognoza tygodniowa");
                            Console.WriteLine("3. Prognoza godzinowa");      // ✅ DODANE
                            Console.WriteLine("4. Analiza historyczna");     // ✅ DODANE
                            Console.Write("Wybór: ");
                            var reportChoice = Console.ReadLine();

                            ReportType reportType = reportChoice switch
                            {
                                "2" => ReportType.WEEKLY_FORECAST,
                                "3" => ReportType.HOURLY_FORECAST,       // ✅ DODANE
                                "4" => ReportType.HISTORICAL_ANALYSIS,   // ✅ DODANE
                                _ => ReportType.CURRENT_CONDITIONS
                            };

                            var report = await app.CreateReport(reportType, city7);
                            if (report != null)
                            {
                                Console.WriteLine(report.ToString());
                            }
                        }
                        break;

                    case "0": // Wyjście
                        Console.WriteLine("\n👋 Do widzenia!");
                        return;

                    default:
                        Console.WriteLine("✗ Nieprawidłowa opcja!");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n❌ BŁĄD: {ex.Message}");
            }

            Console.WriteLine("\nNaciśnij dowolny klawisz aby kontynuować...");
            Console.ReadKey();
            Console.Clear();
        }
    }
}
