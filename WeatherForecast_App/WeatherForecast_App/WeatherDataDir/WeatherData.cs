namespace WeatherForecast_App;

public abstract class WeatherData
{
    public double latitude;
    public double longitude;
    public string timezone;

    public LocationData GetLocation()
    {
        return new LocationData(latitude, longitude);
    }

    public abstract string GetSummary();
}