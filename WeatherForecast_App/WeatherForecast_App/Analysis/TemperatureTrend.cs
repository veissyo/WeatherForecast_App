namespace WeatherForecast_App;

public class TemperatureTrend : IWeatherAnalysis
{
    public string Analyze(DailyWeatherData data)
    {
        var avgTemps = new double[data.time.Length];
        for (int i = 0; i < data.time.Length; i++)
        {
            avgTemps[i] = (data.temperature_2m_max[i] + data.temperature_2m_min[i]) / 2;
        }

        var trend = CalculateTrend(avgTemps);
        var avgTemp = avgTemps.Average();
        var maxTemp = data.temperature_2m_max.Max();
        var minTemp = data.temperature_2m_min.Min();
        var tempRange = maxTemp - minTemp;

        var analysis = $"Average temperature: {avgTemp:F1} C\n";
        analysis += $"Temperature range: {minTemp:F1} C to {maxTemp:F1} C (span: {tempRange:F1} C)\n";
        analysis += $"Trend: {trend}\n";

        return analysis;
    }

    private string CalculateTrend(double[] temps)
    {
        if (temps.Length < 2) return "Unknown";

        var firstHalf = temps.Take(temps.Length / 2).Average();
        var secondHalf = temps.Skip(temps.Length / 2).Average();
        var diff = secondHalf - firstHalf;

        if (Math.Abs(diff) < 1.0)
            return "Stable";
        else if (diff > 0)
            return $"Warming (+ {diff:F1} C)";
        else
            return $"Cooling (- {Math.Abs(diff):F1} C)";
    }
}