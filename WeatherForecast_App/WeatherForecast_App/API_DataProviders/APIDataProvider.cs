namespace WeatherForecast_App;

public class APIDataProvider : IDataProvider
{
    private APIClient _client;

    public APIDataProvider()
    {
        _client = APIClient.GetInstance();
    }

    public async Task<WeatherData> GetWeatherData(LocationData location, WeatherServiceType serviceType)
    {
        var url = BuildURL(location, serviceType);

        switch (serviceType)
        {
            case WeatherServiceType.CURRENT:
                var response = await _client.GetAsync<CurrentWeatherResponse>(url);
                if (response.current != null)
                {
                    response.current.latitude = response.latitude;
                    response.current.longitude = response.longitude;
                    response.current.timezone = response.timezone;
                    return response.current;
                }
                throw new Exception("Failed to fetch current weather data. ;(");
            
                case WeatherServiceType.DAILY_7:
                var dailyResponse = await _client.GetAsync<DailyWeatherResponse>(url);
                if (dailyResponse.daily != null)
                {
                    dailyResponse.daily.latitude = dailyResponse.latitude;
                    dailyResponse.daily.longitude = dailyResponse.longitude;
                    dailyResponse.daily.timezone = dailyResponse.timezone;
                    return dailyResponse.daily;
                }
                throw new Exception("Failed to fetch daily weather data. ;(");
                
            case WeatherServiceType.HOURLY_24:
                var hourlyResponse = await _client.GetAsync<HourlyWeatherResponse>(url);
                if (hourlyResponse.hourly != null)
                {
                    hourlyResponse.hourly.latitude = hourlyResponse.latitude;
                    hourlyResponse.hourly.longitude = hourlyResponse.longitude;
                    hourlyResponse.hourly.timezone = hourlyResponse.timezone;
                    return hourlyResponse.hourly;
                }
                throw new Exception("Failed to fetch hourly weather data. ;(");
                
            case WeatherServiceType.HISTORICAL:
                var historicalResponse = await _client.GetAsync<HistoricalWeatherResponse>(url);
                if (historicalResponse.daily != null)
                {
                    var historical = new HistoricalWeatherData
                    {
                        latitude = historicalResponse.latitude,
                        longitude = historicalResponse.longitude,
                        timezone = historicalResponse.timezone,
                        daily = historicalResponse.daily
                    };
                    historical.daily.latitude = historicalResponse.latitude;
                    historical.daily.longitude = historicalResponse.longitude;
                    historical.daily.timezone = historicalResponse.timezone;
                    return historical;
                }
                throw new Exception("Failed to fetch historical weather data");
            
            default:
                throw new NotImplementedException("Unknown service type. Oops.");
        }
    }

    private string BuildURL(LocationData location, WeatherServiceType serviceType)
    {
        switch (serviceType)
        {
            case WeatherServiceType.CURRENT:
                var url = $"{OpenMeteoEndpoints.FORECAST_API_URL}" +
                          $"?latitude={location.latitude.ToString(System.Globalization.CultureInfo.InvariantCulture)}" +
                          $"&longitude={location.longitude.ToString(System.Globalization.CultureInfo.InvariantCulture)}" +
                          $"&current=temperature_2m,apparent_temperature,rain,snowfall,weather_code,wind_speed_10m,cloud_cover" +
                          $"&timezone=auto";
                return url;
            
            case WeatherServiceType.DAILY_7:
                var urlDaily7 = $"{OpenMeteoEndpoints.FORECAST_API_URL}" +
                         $"?latitude={location.latitude.ToString(System.Globalization.CultureInfo.InvariantCulture)}" +
                         $"&longitude={location.longitude.ToString(System.Globalization.CultureInfo.InvariantCulture)}" +
                         $"&daily=temperature_2m_max,temperature_2m_min,rain_sum,snowfall_sum,wind_speed_10m_max" +
                         $"&forecast_days=7" +
                         $"&timezone=auto";
                return urlDaily7;
                
            case WeatherServiceType.HOURLY_24:
                var urlHourly = $"{OpenMeteoEndpoints.FORECAST_API_URL}" +
                         $"?latitude={location.latitude.ToString(System.Globalization.CultureInfo.InvariantCulture)}" +
                         $"&longitude={location.longitude.ToString(System.Globalization.CultureInfo.InvariantCulture)}" +
                         $"&hourly=temperature_2m,rain,snowfall,weather_code,cloud_cover,wind_speed_10m" +
                         $"&forecast_days=1" +
                         $"&timezone=auto";
                return urlHourly;
                
            case WeatherServiceType.HISTORICAL:
                var endDate = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
                var startDate = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
                var urlHistorical = $"{OpenMeteoEndpoints.ARCHIVE_API_URL}" +
                         $"?latitude={location.latitude.ToString(System.Globalization.CultureInfo.InvariantCulture)}" +
                         $"&longitude={location.longitude.ToString(System.Globalization.CultureInfo.InvariantCulture)}" +
                         $"&start_date={startDate}" +
                         $"&end_date={endDate}" +
                         $"&daily=temperature_2m_max,temperature_2m_min,rain_sum,snowfall_sum,wind_speed_10m_max" +
                         $"&timezone=auto";
                return urlHistorical;
            
            default:
                throw new NotImplementedException($"URL building for this service doesn't exist. Oops.");
        }
    }
}