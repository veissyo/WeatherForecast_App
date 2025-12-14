namespace WeatherForecast_App;

public class HourlyWeatherData : WeatherData
{
    public string[] time { get; set; } 
    public double[] temperature_2m { get; set; }
    public double[] rain { get; set; } 
    public double[] snowfall { get; set; }
    public int[] weather_code { get; set; }
    public double[] cloud_cover { get; set; }
    public double[] wind_speed_10m { get; set; }

    public override string GetSummary()
    {
        if (time.Length == 0) return "No data to display.";
        var summary = $"Forecast for {time.Length} hours:\n";
        for (int i = 0; i < time.Length; i++)
        {
            var desc = WeatherCodeHelper.GetDescription(weather_code[i]);
            summary+= $"  {time[i]}: {temperature_2m[i]:F1}°C, {desc}\n";
        }

        return summary;
    }
}

public class HourlyWeatherResponse
{
    public double latitude { get; set; }
    public double longitude { get; set; }
    public string timezone { get; set; } = "UTC";
    public HourlyWeatherData? hourly { get; set; }
}