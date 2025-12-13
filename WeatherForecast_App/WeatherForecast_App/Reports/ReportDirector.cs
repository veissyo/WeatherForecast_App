namespace WeatherForecast_App;

public class ReportDirector
{
    private readonly IWeatherReportBuilder _builder;

    public ReportDirector(IWeatherReportBuilder builder)
    {
        _builder = builder;
    }

    public WeatherReport BuildCurrentConditionsReport(LocationData location, CurrentWeatherData data)
    {
        return _builder.Reset().SetLocation(location).AddCurrentConditions(data).GetResult();
    }

    public WeatherReport BuildWeeklyForecastReport(LocationData location, DailyWeatherData data)
    {
        return _builder.Reset().SetLocation(location).AddForecast(data).AddMinMaxTemperatures()
            .GetResult();
    }
    
    public WeatherReport BuildHourlyForecastReport(LocationData location, HourlyWeatherData data)
    {
        return _builder.Reset().SetLocation(location).AddForecast(data).GetResult();
    }

    public WeatherReport BuildHistoricalReport(LocationData location, HistoricalWeatherData data)
    {
        return _builder.Reset().SetLocation(location).AddHistoricalData(data).GetResult();
    }

    public WeatherReport BuildComparisonReport(List<LocationData> locations, List<WeatherData> data) // implement
    {
        _builder.Reset();
        _builder.GetResult().Title = "Comparison raport";
        
        var report = _builder.GetResult();
        for (int i = 0; i < locations.Count; i++)
        {
            if (data[i] is CurrentWeatherData current)
            {
                report.Content += $" Location {i + 1} ({locations[i].latitude:F2}, {locations[i].longitude:F2}):\n";
                report.Content += $" Temperature: {current.temperature_2m:F1}°C\n";
                report.Content += $" {WeatherCodeHelper.GetDescription(current.weather_code)}\n\n";
            }
        }
        
        return report;
    }
}