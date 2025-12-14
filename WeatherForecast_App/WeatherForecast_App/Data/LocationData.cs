namespace WeatherForecast_App;

public class LocationData
{
    public double longitude { get; set; }
    public double latitude { get; set; }
    
    public LocationData() { }
    
    public LocationData(double lat, double lon)
    {
        latitude = lat;
        longitude = lon;
    }
}

public class GeocodingResponse
{
    public List<LocationData>? results { get; set; }
}