namespace WeatherForecast_App;

public interface IWeatherAnalysis
{
    string Analyze(DailyWeatherData data);
}