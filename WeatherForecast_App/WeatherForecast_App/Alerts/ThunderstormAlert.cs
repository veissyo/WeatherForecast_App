namespace WeatherForecast_App;

public class ThunderstormAlert : WeatherAlert
{
    public ThunderstormAlert(string location) : base(location) { }

    protected override bool CheckCondition(WeatherData data)
    {
        if (data is HourlyWeatherData hourly)
        {
            int hoursToCheck = Math.Min(6, hourly.weather_code.Length); // checks the next 6 hours to see if a thunderstorm is expected

            for (int i = 0; i < hoursToCheck; i++)
            {
                if (WeatherCodeHelper.IsThunderstorm(hourly.weather_code[i])) // weather code says if it's thundering rn
                {
                    return true;
                }
            }
        }
        return false; // if no thunderstorm is expected, return false
    }

    protected override string GetAlertMessage(WeatherData data)
    {
        if (data is HourlyWeatherData hourly)
        {
            for (int i = 0; i < Math.Min(24, hourly.weather_code.Length); i++) // checking when exactly the thunderstorm is expected
            {
                if (WeatherCodeHelper.IsThunderstorm(hourly.weather_code[i]))
                {
                    DateTime forecastTime = DateTime.Parse(hourly.time[i]);
                    DateTime now = DateTime.Now;
                    TimeSpan timeUntil = forecastTime - now;
                    
                    if (timeUntil.TotalMinutes < 0) // prevents things like -8 hours from showing up
                    {
                        continue;
                    }
                    
                    return $"THUNDERSTORM FORECAST in {timeUntil.Hours}h {timeUntil.Minutes}min\n" +
                           $"Expected time: {hourly.time[i]}\n" +
                           $"Expected temp: {hourly.temperature_2m[i]:F1}°C\n" +
                           $"Expected wind: {hourly.wind_speed_10m[i]:F1} km/h";
                }
            }
        }
        return "Thunderstorm detected!";
    }
}