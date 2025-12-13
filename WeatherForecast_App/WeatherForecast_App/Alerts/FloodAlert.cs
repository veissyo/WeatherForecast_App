namespace WeatherForecast_App;

public class FloodAlert : WeatherAlert
{
    public FloodAlert(string location) : base(location) { }

    protected override bool CheckCondition(WeatherData data)
    {
        if (data is not CurrentWeatherData current) return false;
        return WeatherCodeHelper.IsHeavyRain(current.weather_code);
    }

    protected override string GetAlertMessage(WeatherData data)
    {
        var current = (CurrentWeatherData)data;
        return $"Warning! Intensive rainfall expected. Current rainfall amount: {current.rain:F1}";
    }
}