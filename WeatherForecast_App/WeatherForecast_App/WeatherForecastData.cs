namespace WeatherForecast_App;

public class WeatherForecastData
{
    public hourly? hourly { get; set; }
    public current? current { get; set; }
}

public class hourly
{
    public string[]? time { get; set; }
    public double[]? temperature_2m { get; set; }
    public double[]? rain { get; set; }
    public double[]? apparent_temperature { get; set; }
    public double[]? snowfall { get; set; }

}

public class current
{
    public double temperature_2m { get; set; }
    public double rain { get; set; }
    public double snowfall { get; set; }
    public double cloud_cover { get; set; }
}