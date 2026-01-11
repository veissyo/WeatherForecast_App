namespace WeatherForecast_App;

public class FloodAlert : WeatherAlert
{
    public FloodAlert(string location) : base(location) { }

    protected override bool CheckCondition(WeatherData data)
    {
        if (data is HourlyWeatherData hourly)
        {
            int hoursToCheck = Math.Min(6, hourly.rain.Length); // checks the next 6 hours
            
            for (int i = 0; i < hoursToCheck; i++)
            {
                if (hourly.rain[i] > 10.0) // > 10mm/h is heavy rain
                {
                    return true;
                }
            }

            for (int i = 0; i < hoursToCheck; i++)
            {
                if (WeatherCodeHelper.IsHeavyRain(hourly.weather_code[i]))
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
            for (int i = 0; i < Math.Min(24, hourly.rain.Length); i++) // checking when exactly the heavy rain is expected
            {
                if (hourly.rain[i] > 10.0 || WeatherCodeHelper.IsHeavyRain(hourly.weather_code[i]))
                {
                    DateTime forecastTime = DateTime.Parse(hourly.time[i]);
                    DateTime now = DateTime.Now;
                    TimeSpan timeUntil = forecastTime - now;
                    
                    if (timeUntil.TotalMinutes < 0)
                    {
                        continue;
                    }

                    return $"FLOOD RISK - HEAVY RAIN FORECAST in {timeUntil.Hours}h {timeUntil.Minutes}min\n" +
                           $"Expected time: {hourly.time[i]}\n" +
                           $"Expected rainfall: {hourly.rain[i]:F1}mm/h";
                    
                }
            }
        }
        return "Flood risk detected to heavy rain!";
    }
}