namespace WeatherForecast_App;

public class WeatherHistoryMemento // keeps a snapshot of the weather data and returns it if needed
{
    public WeatherData Data; // the weather data
    public DateTime Timestamp; // when the snapshot was taken
    public LocationData Location;
    public WeatherServiceType ServiceType;

    public WeatherHistoryMemento(WeatherData data, LocationData location, WeatherServiceType serviceType)
    {
        Data = data;
        Location = location;
        Timestamp = DateTime.Now;
    }
}