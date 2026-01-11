using WeatherForecast_App;

public class TemperatureAlert : WeatherAlert
{
    private readonly double _threshold;
    private readonly bool _isAbove;
    
    public TemperatureAlert(string location, double threshold, bool isAbove) : base(location)
    {
        _threshold = threshold;
        _isAbove = isAbove;
    }
    
    protected override bool CheckCondition(WeatherData data)
    {
        if (data is HourlyWeatherData hourly)
        {
            int hoursToCheck = Math.Min(6, hourly.weather_code.Length); // checks the next 6 hours 

            for (int i = 0; i < hoursToCheck; i++)
            {
                double temp = hourly.temperature_2m[i];

                if (_isAbove && temp > _threshold)
                {
                    return true;
                }
                else if (!_isAbove && temp < _threshold)
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
            for (int i = 0; i < Math.Min(24, hourly.temperature_2m.Length); i++) // checking when exactly the temperature is expected
            {
                double temp = hourly.temperature_2m[i];
                bool condition = _isAbove ? temp > _threshold : temp < _threshold;

                if (condition)
                {
                    DateTime forecastTime = DateTime.Parse(hourly.time[i]);
                    DateTime now = DateTime.Now;
                    TimeSpan timeUntil = forecastTime - now; // checks how long until the temperature is expected

                    if (timeUntil.TotalMinutes < 0)
                    {
                        continue;
                    }
                    
                    string direction = _isAbove ? "ABOVE" : "BELOW";

                    return
                        $"TEMPERATURE {direction} {_threshold}°C FORECAST in {timeUntil.Hours}h {timeUntil.Minutes}min\n" +
                        $"Expected time: {hourly.time[i]}\n" +
                        $"Expected temperature: {temp:F1}°C";
                }
            }
        }
        string dir = _isAbove ? "above" : "below";
        return $"Temperature {dir} {_threshold}°C detected!";
    }
}