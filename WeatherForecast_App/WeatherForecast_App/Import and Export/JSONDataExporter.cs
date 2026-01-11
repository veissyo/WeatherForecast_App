using System.Text.Json;
using System.Text.Json.Serialization;

namespace WeatherForecast_App;

public class JSONDataExporter
{
    private readonly JsonSerializerOptions _options;

    public JSONDataExporter()
    {
        _options = new JsonSerializerOptions
        {
            WriteIndented = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            IncludeFields = true
        };
    }

    public async Task ExportWeatherData(WeatherData data, string filePath, string cityName)
    {
        object exportData; // an object to hold the data to be serialized and exported

        if (data is CurrentWeatherData current)
        {
            exportData = new
            {
                CityName = cityName,
                ExportedAt = DateTime.Now,
                DataType = "CurrentWeatherData",
                Location = new
                {
                    Latitude = current.latitude,
                    Longitude = current.longitude,
                    Timezone = current.timezone
                },
                WeatherData = new
                {
                    time = current.time,
                    temperature_2m = current.temperature_2m,
                    apparent_temperature = current.apparent_temperature,
                    rain = current.rain,
                    snowfall = current.snowfall,
                    weather_code = current.weather_code,
                    wind_speed_10m = current.wind_speed_10m,
                    cloud_cover = current.cloud_cover
                }
            };
        }
        else if (data is DailyWeatherData daily)
        {
            exportData = new
            {
                CityName = cityName,
                ExportedAt = DateTime.Now,
                DataType = "DailyWeatherData",
                Location = new
                {
                    Latitude = daily.latitude,
                    Longitude = daily.longitude,
                    Timezone = daily.timezone
                },
                WeatherData = new
                {
                    time = daily.time,
                    temperature_2m_max = daily.temperature_2m_max,
                    temperature_2m_min = daily.temperature_2m_min,
                    rain_sum = daily.rain_sum,
                    snowfall_sum = daily.snowfall_sum,
                    wind_speed_10m_max = daily.wind_speed_10m_max
                }
            };
        }
        else if (data is HourlyWeatherData hourly)
        {
            exportData = new
            {
                CityName = cityName,
                ExportedAt = DateTime.Now,
                DataType = "HourlyWeatherData",
                Location = new
                {
                    Latitude = hourly.latitude,
                    Longitude = hourly.longitude,
                    Timezone = hourly.timezone
                },
                WeatherData = new
                {
                    time = hourly.time,
                    temperature_2m = hourly.temperature_2m,
                    rain = hourly.rain,
                    snowfall = hourly.snowfall,
                    weather_code = hourly.weather_code,
                    cloud_cover = hourly.cloud_cover,
                    wind_speed_10m = hourly.wind_speed_10m
                }
            };
        }
        else if (data is HistoricalWeatherData historical)
        {
            exportData = new
            {
                CityName = cityName,
                ExportedAt = DateTime.Now,
                DataType = "HistoricalWeatherData",
                Location = new
                {
                    Latitude = historical.latitude,
                    Longitude = historical.longitude,
                    Timezone = historical.timezone
                },
                WeatherData = historical.daily != null
                    ? new
                    {
                        time = historical.daily.time,
                        temperature_2m_max = historical.daily.temperature_2m_max,
                        temperature_2m_min = historical.daily.temperature_2m_min,
                        rain_sum = historical.daily.rain_sum,
                        snowfall_sum = historical.daily.snowfall_sum,
                        wind_speed_10m_max = historical.daily.wind_speed_10m_max
                    }
                    : null
            };
        }
        else
        {
            exportData = new
            {
                CityName = cityName,
                ExportedAt = DateTime.Now,
                DataType = data.GetType().Name,
                Location = new
                {
                    Latitude = data.latitude,
                    Longitude = data.longitude,
                    Timezone = data.timezone
                },
                WeatherData = data
            };
        }

        var json = JsonSerializer.Serialize(exportData, _options); // serializes the data to JSON
        await File.WriteAllTextAsync(filePath, json); // writes the JSON to a file, async so it doesn't block the running program
        Console.WriteLine($"Weather data exported to: {filePath}");
    }

    public async Task ExportWatchedLocations(Dictionary<string, WatchedLocation> locations, string filePath)
    {
        var exportData = new
        {
            ExportedAt = DateTime.Now,
            LocationsCount = locations.Count,
            Locations = locations.Select(kvp => new
            {
                CityName = kvp.Key,
                Latitude = kvp.Value._locationData.latitude,
                Longitude = kvp.Value._locationData.longitude,
                AlertsCount = kvp.Value.GetAlerts().Count,
                Alerts = kvp.Value.GetAlerts().Select(a => new
                {
                    Type = a.GetType().Name,
                    Location = a._locationName
                }).ToList()
            }).ToList()
        };

        var json = JsonSerializer.Serialize(exportData, _options);
        await File.WriteAllTextAsync(filePath, json);
        Console.WriteLine($"Observed locations exported to: {filePath}");
    }

    public async Task ExportWeatherReport(WeatherReport report, string filePath)
    {
        var exportData = new
        {
            report.Title,
            report.Type,
            report.GeneratedAt,
            Location = report.Location != null ? new {
                    report.Location.latitude,
                    report.Location.longitude } : null,
            report.Content
        };

        var json = JsonSerializer.Serialize(exportData, _options);
        await File.WriteAllTextAsync(filePath, json);
        Console.WriteLine($"Report exported to: {filePath}");
    }
}