using UnityEngine;

public class SkyboxChanger : MonoBehaviour
{
    public Material clearSkybox;
    public Material cloudySkybox;
    public Material thunderstormSkybox;
    public Material nightClearSkybox;
    public Material nightCloudySkybox;
    public Material nightThunderstormSkybox;

    public void UpdateSkybox(WeatherCondition condition, bool isDay)
    {
        switch (condition)
        {
            case WeatherCondition.Clear:
                SetSkybox(isDay ? clearSkybox : nightClearSkybox);
                break;
            case WeatherCondition.PartlyCloudy:
                SetSkybox(isDay ? cloudySkybox : nightCloudySkybox);
                break;
            case WeatherCondition.Cloudy:
                SetSkybox(isDay ? cloudySkybox : nightCloudySkybox);
                break;
            case WeatherCondition.Rain:
                SetSkybox(isDay ? cloudySkybox : nightCloudySkybox);
                break;
            case WeatherCondition.Thunderstorm:
                SetSkybox(isDay ? thunderstormSkybox : nightThunderstormSkybox);
                break;
        }
    }

    static void SetSkybox(Material skybox)
    {
        if (skybox == null)
            return;

        RenderSettings.skybox = skybox;
        DynamicGI.UpdateEnvironment();
    }
}
