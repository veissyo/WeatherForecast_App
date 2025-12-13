namespace WeatherForecast_App;

public class WeatherHistoryMemento
{
    public WeatherData Data;
    public DateTime Timestamp;
    public LocationData Location;

    public WeatherHistoryMemento(WeatherData data, LocationData location)
    {
        Data = data;
        Location = location;
        Timestamp = DateTime.Now;
    }
}