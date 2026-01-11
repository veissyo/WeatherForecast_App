using System.Text.Json;

namespace WeatherForecast_App;

public class JSONDataImporter 
{
    public async Task<WeatherData?> ImportWeatherData(string filePath)
    {
        try
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"File doesn't exist: {filePath}");
                return null;
            }

            var json = await File.ReadAllTextAsync(filePath);
            var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            if (!root.TryGetProperty("DataType", out var dataTypeElement))
            {
                Console.WriteLine("No information about data type found in file.");
                return null;
            }

            var dataType = dataTypeElement.GetString();
            var weatherDataElement = root.GetProperty("WeatherData");
            var locationElement = root.GetProperty("Location");

            var latitude = locationElement.GetProperty("Latitude").GetDouble();
            var longitude = locationElement.GetProperty("Longitude").GetDouble();
            var timezone = locationElement.GetProperty("Timezone").GetString() ?? "UTC";

            WeatherData? data = dataType switch
            {
                "CurrentWeatherData" => JsonSerializer.Deserialize<CurrentWeatherData>(weatherDataElement.GetRawText()),
                "DailyWeatherData" => JsonSerializer.Deserialize<DailyWeatherData>(weatherDataElement.GetRawText()),
                "HourlyWeatherData" => JsonSerializer.Deserialize<HourlyWeatherData>(weatherDataElement.GetRawText()),
                "HistoricalWeatherData" => DeserializeHistoricalData(weatherDataElement, latitude, longitude, timezone),
                _ => null
            };

            if (data != null)
            {
                data.latitude = latitude;
                data.longitude = longitude;
                data.timezone = timezone;
                Console.WriteLine($"Successfully imported data from: {filePath}");
            }

            return data;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Import error: {ex.Message}");
            return null;
        }
    }

    private HistoricalWeatherData? DeserializeHistoricalData(JsonElement weatherDataElement, double latitude, double longitude, string timezone)
    {
        if (weatherDataElement.ValueKind == JsonValueKind.Null)
        {
            return null;
        }

        var dailyData = JsonSerializer.Deserialize<DailyWeatherData>(weatherDataElement.GetRawText());
        if (dailyData == null) return null;

        dailyData.latitude = latitude;
        dailyData.longitude = longitude;
        dailyData.timezone = timezone;

        return new HistoricalWeatherData
        {
            latitude = latitude,
            longitude = longitude,
            timezone = timezone,
            daily = dailyData
        };
    }

    public async Task<Dictionary<string, LocationData>?> ImportWatchedLocations(string filePath)
    {
        try
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"File doesn't exist: {filePath}");
                return null;
            }

            var json = await File.ReadAllTextAsync(filePath);
            var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            if (!root.TryGetProperty("Locations", out var locationsElement))
            {
                Console.WriteLine("No location data found in file.");
                return null;
            }

            var locations = new Dictionary<string, LocationData>();

            foreach (var locationElement in locationsElement.EnumerateArray())
            {
                var cityName = locationElement.GetProperty("CityName").GetString();
                var latitude = locationElement.GetProperty("Latitude").GetDouble();
                var longitude = locationElement.GetProperty("Longitude").GetDouble();

                if (cityName != null)
                {
                    locations[cityName] = new LocationData(latitude, longitude);
                }
            }

            Console.WriteLine($"Successfully imported {locations.Count} locations from: {filePath}");
            return locations;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Import error: {ex.Message}");
            return null;
        }
    }
}