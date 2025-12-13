namespace WeatherForecast_App;

public interface IWeatherReportBuilder
{
    IWeatherReportBuilder Reset();
    IWeatherReportBuilder SetLocation(LocationData location);
    IWeatherReportBuilder AddCurrentConditions(CurrentWeatherData data);
    IWeatherReportBuilder AddForecast(WeatherData data);
    IWeatherReportBuilder AddHistoricalData(HistoricalWeatherData data);
    IWeatherReportBuilder AddMinMaxTemperatures();
    WeatherReport GetResult();
}