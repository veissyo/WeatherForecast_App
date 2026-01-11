namespace WeatherForecast_App;

public class FogAlert : WeatherAlert
{
    public FogAlert(string location) : base(location) { }

    protected override bool CheckCondition(WeatherData data)
    {
        if (data is HourlyWeatherData hourly)
        {
            int hoursToCheck = Math.Min(6, hourly.weather_code.Length); // checks the next 6 hours 
            
            for (int i = 0; i < hoursToCheck; i++)
            {
                if (WeatherCodeHelper.IsFog(hourly.weather_code[i]))
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
            for (int i = 0; i < Math.Min(24, hourly.weather_code.Length); i++) // checking when exactly the fog is expected
            {
                if (WeatherCodeHelper.IsFog(hourly.weather_code[i]))
                {
                    DateTime forecastTime = DateTime.Parse(hourly.time[i]);
                    DateTime now = DateTime.Now;
                    TimeSpan timeUntil = forecastTime - now;
                    
                    if (timeUntil.TotalMinutes < 0)
                    {
                        continue;
                    }
                    
                    return $"FOG FORECAST in {timeUntil.Hours}h {timeUntil.Minutes}min\n" + 
                           $"Expected time: {hourly.time[i]}\n" +
                           $"Visibility will be reduced - plan accordingly!";
                }
            }
        }
        return "Fog detected!";
    }
}