namespace WeatherForecast_App;

public class HistoricalWeatherData : WeatherData
{
    public string[] start_date { get; set; }
    public string[] end_date { get; set; }
    public DailyWeatherData? daily { get; set; }

    public HistoricalWeatherData()
    {
        daily = new DailyWeatherData();
    }
    
    public override string GetSummary()
    {
        return $"Dane historyczne od {start_date:yyyy-MM-dd} do {end_date:yyyy-MM-dd}\n" + daily.GetSummary();
    }
}

public class HistoricalWeatherResponse
{
    public double latitude { get; set; }
    public double longitude { get; set; }
    public string timezone { get; set; } = "UTC";
    public DailyWeatherData? daily { get; set; }
}