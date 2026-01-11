namespace WeatherForecast_App;

public class SnowAlert : WeatherAlert
{
    public SnowAlert(string location) : base(location) {}

    protected override bool CheckCondition(WeatherData data)
    {
        if (data is HourlyWeatherData hourly)
        {
            int hoursToCheck = Math.Min(6, hourly.snowfall.Length); // checks the next 6 hours
            
            for (int i = 0; i < hoursToCheck; i++)
            {
                if (hourly.snowfall[i] > 1.0 || WeatherCodeHelper.IsSnow(hourly.weather_code[i]))
                {
                    return true;
                }
            }
        }

        return false;
    }

    protected override string GetAlertMessage(WeatherData data)
    {
        if (data is HourlyWeatherData hourly)
        {
            for (int i = 0; i < Math.Min(24, hourly.snowfall.Length); i++) // checking when exactly the snow is expected
            {
                if (hourly.snowfall[i] > 1.0 || WeatherCodeHelper.IsSnow(hourly.weather_code[i]))
                {
                    DateTime forecastTime = DateTime.Parse(hourly.time[i]);
                    DateTime now = DateTime.Now;
                    TimeSpan timeUntil = forecastTime - now;
                    
                    if (timeUntil.TotalMinutes < 0)
                    {
                        continue;
                    }
                    
                    return $"SNOW FORECAST in {timeUntil.Hours}h {timeUntil.Minutes}min\n" +
                           $"Expected time: {hourly.time[i]}\n" +
                           $"xpected snowfall: {hourly.snowfall[i]:F1}cm";
                   
                }
            }
        }
        return "Snowfall has been detected.";
    }
}