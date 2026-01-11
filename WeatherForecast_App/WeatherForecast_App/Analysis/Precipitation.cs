namespace WeatherForecast_App;
public class Precipitation : IWeatherAnalysis
{
    public string Analyze(DailyWeatherData data)
    {
        if (data.time.Length == 0) return "No data available.";

        var totalRain = data.rain_sum.Sum();
        var totalSnow = data.snowfall_sum.Sum();
        var rainyDays = data.rain_sum.Count(r => r > 0.1);
        var snowyDays = data.snowfall_sum.Count(s => s > 0.1);

        var analysis = $"Total rainfall: {totalRain:F1} mm over {rainyDays} days\n";
        analysis += $"Total snowfall: {totalSnow:F1} mm over {snowyDays} days\n";

        if (rainyDays > 0)
        {
            var avgRainPerDay = totalRain / rainyDays;
            analysis += $"Average rain per rainy day: {avgRainPerDay:F1} mm\n";
        }

        if (totalRain > 50)
            analysis += "Warning: High precipitation expected.\n";
        else if (totalRain < 5)
            analysis += "Low or no precipitation expected.\n";

        return analysis;
    }
}