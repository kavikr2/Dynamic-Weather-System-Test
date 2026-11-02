using System;

public enum WeatherCondition
{
    Clear,
    PartlyCloudy,
    Cloudy,
    Fog,
    Drizzle,
    Rain,
    Snow,
    Thunderstorm
}

public class GeoLocation
{
    public double Latitude;
    public double Longitude;
    public string Name;
}

[Serializable]
public class WeatherReport
{
    public string locationName;
    public float temperatureC;
    public float windSpeedKmh;
    public bool isDay;
    public string localTime;
    public WeatherCondition condition;
}


[Serializable]
public class OpenMeteoResponse
{
    public OpenMeteoCurrent current;
}

[Serializable]
public class OpenMeteoCurrent
{
    public string time; 
    public float temperature_2m;
    public float wind_speed_10m;
    public int weather_code;
    public int is_day;
}

[Serializable]
public class IpLocationResponse
{
    public bool success;
    public string message;
    public double latitude;
    public double longitude;
    public string city;
    public string country;
}

public static class WeatherCodes
{
    //https://open-meteo.com/en/docs - doc for weather code 
    public static WeatherCondition ToCondition(int code)
    {
        if (code == 0)
            return WeatherCondition.Clear;

        if (code == 1 || code == 2)
            return WeatherCondition.PartlyCloudy;

        if (code == 3)
            return WeatherCondition.Cloudy;

        if (code == 45 || code == 48)
            return WeatherCondition.Fog;

        if (code >= 51 && code <= 57)
            return WeatherCondition.Drizzle;

        if ((code >= 61 && code <= 67) || (code >= 80 && code <= 82))
            return WeatherCondition.Rain;

        if ((code >= 71 && code <= 77) || code == 85 || code == 86)
            return WeatherCondition.Snow;

        if (code >= 95)
            return WeatherCondition.Thunderstorm;

        return WeatherCondition.Clear; // default weather 
    }
}
