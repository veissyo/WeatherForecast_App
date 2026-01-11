namespace WeatherForecast_App;

public class WeatherAnalyzer
{
    private IWeatherAnalysis? _strategy;

    public void SetStrategy(IWeatherAnalysis strategy)
    {
        _strategy = strategy; // sets a strategy for the analyzer
    }

    public string PerformAnalysis(DailyWeatherData data)
    {
        return _strategy.Analyze(data); // calls the strategy's Analyze method
    }

}