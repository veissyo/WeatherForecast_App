namespace WeatherForecast_App;

public class WeatherAnalyzer
{
    private IWeatherAnalysis? _strategy;

    public void SetStrategy(IWeatherAnalysis strategy)
    {
        _strategy = strategy;
    }

    public string PerformAnalysis(DailyWeatherData data)
    {
        return _strategy.Analyze(data);
    }

}