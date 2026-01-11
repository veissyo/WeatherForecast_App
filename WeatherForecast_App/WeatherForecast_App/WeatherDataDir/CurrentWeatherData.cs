namespace WeatherForecast_App;

public class CurrentWeatherData : WeatherData
{
    public string time { get; set; }
    public double temperature_2m { get; set; }
    public double apparent_temperature { get; set; }
    public double rain { get; set; }
    public double snowfall  { get; set; }
    public int weather_code { get; set; }
    public double wind_speed_10m { get; set; }
    public double cloud_cover { get; set; }

    public override string GetSummary()
    {
        var description = WeatherCodeHelper.GetDescription(weather_code);
        return $"{description}, {temperature_2m:F1}°C (feels like {apparent_temperature:F1}°C), " +
               $"Wind: {wind_speed_10m:F1} km/h, Cloud cover: {cloud_cover}%";
    }
    
    public string GetFormattedTime()
    {
        if (DateTime.TryParse(time, out DateTime dateTime))
        {
            return dateTime.ToString("yyyy-MM-dd HH:mm");
        }
        return time;
    }
}

public class CurrentWeatherResponse
{
    public double latitude { get; set; }
    public double longitude { get; set; }
    public string timezone { get; set; } = "UTC";
    public CurrentWeatherData? current { get; set; }
}