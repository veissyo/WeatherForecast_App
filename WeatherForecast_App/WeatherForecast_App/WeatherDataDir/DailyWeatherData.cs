namespace WeatherForecast_App;

public class DailyWeatherData : WeatherData
{
    public string[] time { get; set; }
    public double[] temperature_2m_max { get; set; } 
    public double[] temperature_2m_min { get; set; } 
    public double[] rain_sum { get; set; } 
    public double[] snowfall_sum { get; set; }
    public double[] wind_speed_10m_max { get; set; }

    public override string GetSummary()
    {
        if (time.Length == 0) return "No data to display.";
        
        var summary = $"Forecast for {time.Length} days:\n";
        for (int i = 0; i < time.Length; i++)
        {
            var formattedDate = FormatDate(time[i]);
            summary += $"  {formattedDate}: {temperature_2m_min[i]:F1}°C - {temperature_2m_max[i]:F1}°C, " +
                       $"Rainfall: {rain_sum[i]:F1}mm\n Snowfall: {snowfall_sum[i]:F1}mm\n"
            + $"Maximum wind speed: {wind_speed_10m_max[i]:F1}km/h \n";
        }
        return summary;
    }
    
    private string FormatDate(string dateString)
    {
        if (DateTime.TryParse(dateString, out DateTime date))
        {
            return date.ToString("yyyy-MM-dd");
        }
        return dateString;
    }
}

public class DailyWeatherResponse
{
    public double latitude { get; set; }
    public double longitude { get; set; }
    public string timezone { get; set; } = "UTC";
    public DailyWeatherData? daily { get; set; }
}