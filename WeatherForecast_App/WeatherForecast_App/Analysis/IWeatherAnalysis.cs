namespace WeatherForecast_App;

public interface IWeatherAnalysis // interface needed for strategy pattern
{
    string Analyze(DailyWeatherData data);
}