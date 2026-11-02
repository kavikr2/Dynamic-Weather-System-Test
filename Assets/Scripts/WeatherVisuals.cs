using TMPro;
using UnityEngine;

public class WeatherVisuals : MonoBehaviour
{
    [SerializeField] private TMP_Text locationNameText;
    [SerializeField] private TMP_Text locationTimeText;
    [SerializeField] private TMP_Text locationDayOrNightText;
    [SerializeField] private TMP_Text locationTempText;
    [SerializeField] private TMP_Text locationWeatherText;
    [SerializeField] private TMP_Text locationWindSpeedText;


    public void SetWeatherVisual(WeatherReport report)
    {
        WeatherCondition condition = report.condition;
        string dayOrNight;

        if (report.isDay)
        {
            dayOrNight = "Day"; 
        }
        else
        {
            dayOrNight = "Night"; 
        }
        locationNameText.text = report.locationName;
        locationTimeText.text = report.localTime;
        locationDayOrNightText.text = dayOrNight;
        locationTempText.text = Mathf.Round(report.temperatureC) + "°C";
        locationWeatherText.text = condition.ToString();
        locationWindSpeedText.text = "Wind " + Mathf.Round(report.windSpeedKmh) + " km/h";
    }
}
