namespace WeatherForecast_App;

public class ReportDirector // it builds the reports using weather report builder's methods
{
    private readonly IWeatherReportBuilder _builder;

    public ReportDirector(IWeatherReportBuilder builder)
    {
        _builder = builder; // receives the builder
    }

    public WeatherReport BuildLocationComparisonReport(LocationData location1, LocationData location2, 
        DailyWeatherData data1, DailyWeatherData data2, string city1, string city2)
    {
        return _builder
            .Reset()
            .SetTitle("Weather Comparison Report")
            .SetLocation(location1)
            .AddComparisonHeader(city1, city2, data1.time.Length)
            .AddLocationWeatherSummary(city1, location1, data1)
            .AddLocationWeatherSummary(city2, location2, data2)
            .AddComparisonSummary(data1, data2, city1, city2)
            .GetResult();
    }

    public WeatherReport BuildWeeklyAlertsReport(LocationData location, DailyWeatherData data, 
        string cityName, List<WeatherAlert> alerts)
    {
        return _builder
            .Reset()
            .SetTitle("Weekly Alerts Report")
            .SetLocation(location)
            .AddAlertsHeader(cityName, data.time.Length, alerts.Count)
            .AddDailyConditions(data)
            .AddAlertsSummary(alerts)
            .AddPotentialHazards(data)
            .GetResult();
    }
}