namespace WeatherForecast_App;

class Program
{
    public static async Task Main()
    {
        Console.WriteLine("Witaj w aplikacji pogodowej!");
        GetLocation getLocation = new GetLocation();
        await getLocation.GetLocationName();
        WeatherForecastAPICALL weatherForecastAPICALL = new WeatherForecastAPICALL();
        await weatherForecastAPICALL.Get_Weather_Forecast();
    }
}
