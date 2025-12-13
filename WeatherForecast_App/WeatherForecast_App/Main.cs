using System.Text.Json;

namespace WeatherForecast_App;

class Program
{
    public static async Task Main()
    {
        Console.WriteLine("=== WEATHER FORECAST APP ===\n");

        try
        {
            // 1. Pytamy użytkownika o miasto
            Console.Write("Podaj nazwę miasta: ");
            var cityName = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(cityName))
            {
                Console.WriteLine("Nie podano nazwy miasta!");
                return;
            }

            // 2. Szukamy lokalizacji (współrzędnych) dla miasta
            Console.WriteLine($"\nSzukam lokalizacji dla: {cityName}...");
            var geoService = new GeoLocationService();
            var location = await geoService.GetLocation(cityName);

            if (location == null)
            {
                Console.WriteLine($"Nie znaleziono miasta: {cityName}");
                return;
            }

            Console.WriteLine($"Znaleziono: Lat={location.latitude}, Lon={location.longitude}");

            // 3. Pobieramy aktualną pogodę
            Console.WriteLine("\nPobieram dane pogodowe...");
            var dataProvider = new APIDataProvider();
            var weatherService = new CurrentWeatherService(dataProvider);
            var weatherData = await weatherService.FetchWeather(location);

            // 4. Wyświetlamy pogodę
            if (weatherData is CurrentWeatherData currentWeather)
            {
                Console.WriteLine("\n=== AKTUALNA POGODA ===");
                Console.WriteLine(currentWeather.GetSummary());
                Console.WriteLine($"\nCzas: {currentWeather.time}");
                Console.WriteLine($"Opady deszczu: {currentWeather.rain} mm");
                Console.WriteLine($"Opady śniegu: {currentWeather.snowfall} mm");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\n❌ BŁĄD: {ex.Message}");
        }

        Console.WriteLine("\nNaciśnij dowolny klawisz aby zakończyć...");
        Console.ReadKey();
    }
}
