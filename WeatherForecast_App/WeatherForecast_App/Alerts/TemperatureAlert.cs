using WeatherForecast_App;

public class TemperatureAlert : WeatherAlert
{
    private readonly double _threshold;
    private readonly bool _isAbove;
    
    public TemperatureAlert(string location, double threshold, bool isAbove) : base(location)
    {
        _threshold = threshold;
        _isAbove = isAbove;
    }
    
    protected override bool CheckCondition(WeatherData data)
    {
        if (data is not CurrentWeatherData current) return false;

        if (_isAbove)
            return current.temperature_2m > _threshold;
        
        return current.temperature_2m < _threshold;
    }
    
    protected override string GetAlertMessage(WeatherData data)
    {
        var current = (CurrentWeatherData)data;
        var comparison = _isAbove ? "above" : "under";
        return $"Temperature {comparison} {_threshold}°C! Current: {current.temperature_2m:F1}°C";
    }
}