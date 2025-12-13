namespace WeatherForecast_App;

public class FogAlert : WeatherAlert
{
    public FogAlert(string location) : base(location) { }

    protected override bool CheckCondition(WeatherData data)
    {
        if (data is not CurrentWeatherData current) return false;
        return WeatherCodeHelper.IsThunderstorm(current.weather_code);
    }

    protected override string GetAlertMessage(WeatherData data)
    {
        var current = (CurrentWeatherData)data;
        var description = WeatherCodeHelper.GetDescription(current.weather_code);
        return "Warning: " + description + " Stay alert while driving.";
    }
}