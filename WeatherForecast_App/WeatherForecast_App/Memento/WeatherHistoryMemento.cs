namespace WeatherForecast_App;

public class WeatherHistoryMemento
{
    public WeatherData Data;
    public DateTime Timestamp;
    public LocationData Location;
    public WeatherServiceType ServiceType;

    public WeatherHistoryMemento(WeatherData data, LocationData location, WeatherServiceType serviceType)
    {
        Data = data;
        Location = location;
        Timestamp = DateTime.Now;
    }
}