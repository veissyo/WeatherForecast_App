namespace WeatherForecast_App;

public class WeatherReportBuilder : IWeatherReportBuilder
{
    private WeatherReport _report;
    private readonly List<WeatherData> _weatherData;
    private LocationData? _location;
    
    public WeatherReportBuilder(ReportType type)
    {
        _report = new WeatherReport(type);
        _weatherData = new List<WeatherData>();
    }

    public IWeatherReportBuilder Reset()
    {
        _report = new WeatherReport(_report.Type);
        _weatherData.Clear();
        _location = null;
        return this;
    }

    public IWeatherReportBuilder SetLocation(LocationData location)
    {
        _location = location;
        _report.Location = location;
        return this;
    }

   public IWeatherReportBuilder SetTitle(string title)
    {
        _report.Title = title;
        return this;
    }

    public IWeatherReportBuilder AddComparisonHeader(string city1, string city2, int days)
    {
        _report.Content += $"Comparing: {city1} and {city2}\n";
        _report.Content += $"Period: {days} days\n\n";
        return this;
    }

    public IWeatherReportBuilder AddLocationWeatherSummary(string cityName, LocationData location, DailyWeatherData data)
    {
        _report.Content += $"{cityName}\n";
        _report.Content += $"Location: ({location.latitude:F2}, {location.longitude:F2})\n";
        
        var avg = CalculateAverageTemp(data);
        var rain = data.rain_sum.Sum();
        
        _report.Content += $"Average temperature: {avg:F1} C\n";
        _report.Content += $"Total rainfall: {rain:F1} mm\n";
        _report.Content += $"Temperature range: {data.temperature_2m_min.Min():F1} C to {data.temperature_2m_max.Max():F1} C\n\n";
        
        return this;
    }

    public IWeatherReportBuilder AddComparisonSummary(DailyWeatherData data1, DailyWeatherData data2, string city1, string city2)
    {
        _report.Content += "Comparison\n";
        
        var avg1 = CalculateAverageTemp(data1);
        var avg2 = CalculateAverageTemp(data2);
        var rain1 = data1.rain_sum.Sum();
        var rain2 = data2.rain_sum.Sum();
        
        var tempDiff = avg1 - avg2;
        var rainDiff = rain1 - rain2;
        
        _report.Content += $"Temperature difference: {Math.Abs(tempDiff):F1} C ";
        _report.Content += tempDiff > 0 ? $"({city1} warmer)\n" : $"({city2} warmer)\n";
        _report.Content += $"Rainfall difference: {Math.Abs(rainDiff):F1} mm ";
        _report.Content += rainDiff > 0 ? $"({city1} has higher precipitation)\n" : $"({city2} has higher precipitation)\n";
        
        return this;
    }

    public IWeatherReportBuilder AddAlertsHeader(string cityName, int days, int alertCount)
    {
        _report.Content += $"Location: {cityName}\n";
        _report.Content += $"Analysis period: {days} days\n";
        _report.Content += $"Active alerts: {alertCount}\n\n";
        return this;
    }

    public IWeatherReportBuilder AddDailyConditions(DailyWeatherData data)
    {
        _report.Content += "Daily Conditions\n";
        for (int i = 0; i < data.time.Length; i++)
        {
            _report.Content += $"{data.time[i]}: ";
            _report.Content += $"{data.temperature_2m_min[i]:F1} C - {data.temperature_2m_max[i]:F1} C, ";
            _report.Content += $"Rain: {data.rain_sum[i]:F1} mm, ";
            _report.Content += $"Snow: {data.snowfall_sum[i]:F1} mm\n";
        }
        _report.Content += "\n";
        return this;
    }

    public IWeatherReportBuilder AddAlertsSummary(List<WeatherAlert> alerts)
    {
        _report.Content += "Alert Summary\n";
        if (alerts.Count == 0)
        {
            _report.Content += "No alerts configured for this location.\n";
        }
        else
        {
            _report.Content += $"Monitoring for: ";
            foreach (var alert in alerts)
            {
                var alertType = alert.GetType().Name.Replace("Alert", "");
                _report.Content += $"{alertType}, ";
            }
        }
        _report.Content += "\n";
        return this;
    }

    public IWeatherReportBuilder AddPotentialHazards(DailyWeatherData data)
    {
        _report.Content += "Potential Hazards\n";
        
        var maxTemp = data.temperature_2m_max.Max();
        var minTemp = data.temperature_2m_min.Min();
        var totalRain = data.rain_sum.Sum();
        var totalSnow = data.snowfall_sum.Sum();
        
        var hazards = new List<string>();
        
        if (maxTemp > 30) hazards.Add($"High temperatures (max: {maxTemp:F1} C)");
        if (minTemp < 0) hazards.Add($"Freezing conditions (min: {minTemp:F1} C)");
        if (totalRain > 50) hazards.Add($"Heavy rainfall expected ({totalRain:F1} mm)");
        if (totalSnow > 10) hazards.Add($"Significant snowfall ({totalSnow:F1} mm)");
        
        if (hazards.Count == 0)
        {
            _report.Content += "No significant hazards detected.\n";
        }
        else
        {
            foreach (var hazard in hazards)
            {
                _report.Content += $"- {hazard}\n";
            }
        }
        
        return this;
    }

    public WeatherReport GetResult()
    {
        return _report;
    }

    private double CalculateAverageTemp(DailyWeatherData data)
    {
        var sum = 0.0;
        for (int i = 0; i < data.time.Length; i++)
        {
            sum += (data.temperature_2m_max[i] + data.temperature_2m_min[i]) / 2;
        }
        return sum / data.time.Length;
    }
}
