namespace WeatherForecast_App;

public abstract class WeatherAlert : IWeatherObserver
{
    public string _locationName;
    protected bool _isEnabled;

    protected WeatherAlert(string locationName)
    {
        _locationName = locationName;
        _isEnabled = true;
    }
    
    protected abstract bool CheckCondition(WeatherData data); 
    protected abstract string GetAlertMessage(WeatherData data);
    
    public void Update(WeatherData data)
    {
        if (!_isEnabled) return;
        if (CheckCondition(data))
        {
            Console.WriteLine($"Alert for {_locationName}: {GetAlertMessage(data)}");
        }
    }
}