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
    
    protected abstract bool CheckCondition(WeatherData data); // checks if the alert condition is met
    protected abstract string GetAlertMessage(WeatherData data); // returns the formatted alert message
    
    public void Update(WeatherData data) // updates the data by checking condition and prints an alert message if needed
    {
        if (!_isEnabled) return;
        if (CheckCondition(data)) 
        {
            Console.WriteLine($"Alert for {_locationName}: {GetAlertMessage(data)}");
        }
    }
}