namespace WeatherForecast_App;

public class WeatherCodeHelper // helps with adding short descriptions and checking weather conditions for extreme things like thunderstorms
{
    public static string GetDescription(int code)
    {
        return code switch
        {
            0 => "Clear sky",
            1 => "Mainly clear",
            2 => "Partly cloudy",
            3 => "Overcast",
            45 => "Fog",
            48 => "Depositing rime fog",
            51 => "Light drizzle",
            53 => "Moderate drizzle",
            55 => "Dense drizzle",
            56 => "Light freezing drizzle",
            57 => "Dense freezing drizzle",
            61 => "Slight rain",
            63 => "Moderate rain",
            65 => "Heavy rain",
            66 => "Light freezing rain",
            67 => "Heavy freezing rain",
            71 => "Slight snow fall",
            73 => "Moderate snow fall",
            75 => "Heavy snow fall",
            77 => "Snow grains",
            80 => "Slight rain showers",
            81 => "Moderate rain showers",
            82 => "Violent rain showers",
            85 => "Slight snow showers",
            86 => "Heavy snow showers",
            95 => "Thunderstorm",
            96 => "Thunderstorm with slight hail",
            99 => "Thunderstorm with heavy hail",
            _ => "Unknown"
        };
    }

    public static bool IsThunderstorm(int code)
    {
        return code >= 95 && code <= 99;
    }

    public static bool IsSnow(int code)
    {
        return (code >= 71 && code <= 77) || code == 85 || code == 86;
    }

    public static bool IsHeavyRain(int code)
    {
        return code == 65 || code ==82;
    }

    public static bool IsFog(int code)
    {
        return code == 45 || code == 48;
    }
}