namespace WeatherForecast_App;

public class SnowAlert : WeatherAlert
{
    public SnowAlert(string location) : base(location) {}

    protected override bool CheckCondition(WeatherData data)
    {
        if (data is not CurrentWeatherData current) return false;
        return WeatherCodeHelper.IsSnow(current.weather_code);
    }

    protected override string GetAlertMessage(WeatherData data)
    {
        var current = (CurrentWeatherData)data;
        var description = WeatherCodeHelper.GetDescription(current.weather_code);
        return $"Yay! {description}!";
    }
}