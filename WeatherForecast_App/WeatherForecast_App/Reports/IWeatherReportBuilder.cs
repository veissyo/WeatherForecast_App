namespace WeatherForecast_App;

public interface IWeatherReportBuilder
{
    IWeatherReportBuilder Reset();
    IWeatherReportBuilder SetLocation(LocationData location);
    IWeatherReportBuilder SetTitle(string title);
    IWeatherReportBuilder SetType(ReportType type);
    IWeatherReportBuilder AddComparisonHeader(string city1, string city2, int days);
    IWeatherReportBuilder AddLocationWeatherSummary(string cityName, LocationData location, DailyWeatherData data);
    IWeatherReportBuilder AddComparisonSummary(DailyWeatherData data1, DailyWeatherData data2, string city1, string city2);
    IWeatherReportBuilder AddAlertsHeader(string cityName, int days, int alertCount);
    IWeatherReportBuilder AddDailyConditions(DailyWeatherData data);
    IWeatherReportBuilder AddAlertsSummary(List<WeatherAlert> alerts);
    IWeatherReportBuilder AddPotentialHazards(DailyWeatherData data);
    WeatherReport GetResult();
}