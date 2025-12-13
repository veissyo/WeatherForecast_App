namespace WeatherForecast_App;

public class WeatherReport
{
    public string Title { get; set; } = "";
    public ReportType Type { get; set; }
    public string Content { get; set; } = "";
    public DateTime GeneratedAt { get; set; }
    public LocationData? Location { get; set; }

    public WeatherReport(ReportType type)
    {
        Type = type;
        GeneratedAt = DateTime.Now;
    }

    public override string ToString()
    {
        return $"\n{new string('=', 50)}\n{Title}\n{new string('=', 50)}\n" +
                      $"Generated at: {GeneratedAt:yyyy-MM-dd HH:mm:ss}\n" +
                      $"Location: {Location?.latitude}, {Location?.longitude}\n" +
                      $"{'-',50}\n{Content}\n{'=',50}\n";
    }
}