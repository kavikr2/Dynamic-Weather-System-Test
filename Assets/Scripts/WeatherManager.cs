using System.Globalization;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class WeatherManager : MonoBehaviour
{
    [SerializeField] private LocationProvider locationProvider;
    [SerializeField] private Button refreshButton;
    public SkyboxChanger skyboxChanger;
    public WeatherVisuals weatherVisuals;

    [Header("Settings")]
    [SerializeField] private float refreshMinutes = 15f;


    private void Start()
    {
        refreshButton.onClick.AddListener(RefreshSystem);
        InvokeRepeating(nameof(RefreshSystem), 0f, refreshMinutes * 60f);
    }

    public void RefreshSystem()
    {
        GeoLocation location = locationProvider.GetLocation();
        if (location == null)
        {
            Debug.LogWarning("Couldn't get location");
            return;
        }

        WeatherReport report = FetchWeather(location);
        if (report == null)
            return;

        skyboxChanger.UpdateSkybox(report.condition, report.isDay);
        weatherVisuals.SetWeatherVisual(report);
    }

    private WeatherReport FetchWeather(GeoLocation location)
    {
        string url = "https://api.open-meteo.com/v1/forecast"
                   + "?latitude=" + location.Latitude.ToString(CultureInfo.InvariantCulture)
                   + "&longitude=" + location.Longitude.ToString(CultureInfo.InvariantCulture)
                   + "&current=temperature_2m,weather_code,wind_speed_10m,is_day"
                   + "&timezone=auto";

        using UnityWebRequest request = UnityWebRequest.Get(url);
        request.timeout = 10;
        request.SendWebRequest();

        // wait for the response
        while (!request.isDone) { }

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogWarning("Weather request failed: " + request.error);
            return null;
        }

        OpenMeteoCurrent data = JsonUtility.FromJson<OpenMeteoResponse>(request.downloadHandler.text).current;

        WeatherReport report = new WeatherReport();
        report.locationName = location.Name;
        report.temperatureC = data.temperature_2m;
        report.windSpeedKmh = data.wind_speed_10m;

        if (data.is_day == 1)
            report.isDay = true;
        else
            report.isDay = false;

        report.localTime = data.time.Substring(11, 5);
        report.condition = WeatherCodes.ToCondition(data.weather_code);

        return report;
    }
}
