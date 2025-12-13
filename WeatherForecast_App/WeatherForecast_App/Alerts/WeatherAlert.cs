namespace WeatherForecast_App;

public abstract class WeatherAlert : IWeatherObserver
{
    protected string _locationName;
    protected bool _isEnabled;

    protected WeatherAlert(string locationName)
    {
        _locationName = locationName;
        _isEnabled = true;
    }
    
    protected abstract bool CheckCondition(WeatherData data); 
    protected abstract string GetAlertMessage(WeatherData data);

    protected void SendAlert(string message)
    {
        Console.WriteLine($"\n ALERT! For {_locationName}: {message}");
    }
    public void Update(WeatherData data)
    {
        if (!_isEnabled) return;
        if (CheckCondition(data))
        {
            SendAlert(GetAlertMessage(data));
        }
    }
}