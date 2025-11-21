namespace WeatherForecast_App;

public class LocationData
{
    public results[]? results { get; set; }
}

public class results
{
    public float? longitude { get; set; }
    public float? latitude { get; set; }
}