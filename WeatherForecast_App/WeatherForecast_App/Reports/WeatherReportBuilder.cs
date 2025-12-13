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

    public IWeatherReportBuilder AddCurrentConditions(CurrentWeatherData data)
    {
        _weatherData.Add(data);
        _report.Title = "Report: Current weather conditions:";
        _report.Content += $"\nCURRENT WEATHER:\n";
        _report.Content += $"  {data.GetSummary()}\n";
        _report.Content += $"  Time: {data.time}\n";
        return this;
    }

    public IWeatherReportBuilder AddForecast(WeatherData data)
    {
        _weatherData.Add(data);
        
        if (data is DailyWeatherData daily)
        {
            _report.Title = "Report: Weekly weather:";
            _report.Content += $"\nForecast for {daily.time.Length} days:\n";
            for (int i = 0; i < daily.time.Length; i++)
            {
                _report.Content += $"{daily.time[i]}:" +
                                  $"{daily.temperature_2m_min[i]:F1}°C - {daily.temperature_2m_max[i]:F1}°C, " +
                                  $"Rainfall: {daily.rain_sum[i]:F1} mm\n";
            }
        }
        else if (data is HourlyWeatherData hourly)
        {
            _report.Title = "Report: Hourly forecast:";
            _report.Content += $"\nHOURLY FORECAST (24h):\n";
            for (int i = 0; i < hourly.time.Length; i++)
            {
                var desc = WeatherCodeHelper.GetDescription(hourly.weather_code[i]);
                _report.Content += $"{hourly.time[i]}: {hourly.temperature_2m[i]:F1}°C, {desc}\n";
            }
        }
        
        return this;
    }

    public IWeatherReportBuilder AddHistoricalData(HistoricalWeatherData data)
    {
        _weatherData.Add(data);
        _report.Title = "Historical report data:";
        _report.Content += $"\n ({data.start_date:yyyy-MM-dd} - {data.end_date:yyyy-MM-dd}):\n";
        _report.Content += $"Maximum temperature: {data.daily.temperature_2m_max:F1}°C\n";
        _report.Content += $"Summarised rainfall/snowfall amount: {data.daily.rain_sum:F1} mm\n"; // Add ? snowfall
        return this;
    }

    public IWeatherReportBuilder AddMinMaxTemperatures()
    {
        var dailyData = _weatherData.OfType<DailyWeatherData>().FirstOrDefault();
        if (dailyData != null)
        {
            var maxTemp = dailyData.temperature_2m_max.Max();
            var minTemp = dailyData.temperature_2m_min.Min();
            _report.Content += $"\nMaximum and minimum temperatures:\n";
            _report.Content += $"  Maximum: {maxTemp:F1}°C\n";
            _report.Content += $"  Minimum: {minTemp:F1}°C\n";
        }
        return this;
    }

    public IWeatherReportBuilder AddDaysAboveThreshold(double threshold)
    {
        var dailyData = _weatherData.OfType<DailyWeatherData>().FirstOrDefault();
        if (dailyData != null)
        {
            var count = dailyData.temperature_2m_max.Count(t => t > threshold);
            _report.Content += $"\nDays with temperature above {threshold}°C: {count}\n";
        }
        return this;
    }

    public WeatherReport GetResult()
    {
        return _report;
    }
}
