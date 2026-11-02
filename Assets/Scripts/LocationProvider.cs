using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LocationProvider : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private WeatherManager weatherManager;

    [Header("UI")]
    [SerializeField] private TMP_Dropdown testLocationsDropdown;
    [SerializeField] private Toggle useTestLocationToggle;


    private bool useTestLocation;
    private TestLocations testLocation;

    private const string ipLookupUrl = "https://ipwho.is/";

    private readonly Dictionary<TestLocations, GeoLocation> testLocations = new Dictionary<TestLocations, GeoLocation>
    {
        { TestLocations.Tokyo, new GeoLocation { Latitude = 35.6895, Longitude = 139.6917, Name = "Tokyo, Japan" } },
        { TestLocations.Singapore, new GeoLocation { Latitude = 1.3521, Longitude = 103.8198, Name = "Singapore" } },
        { TestLocations.Moscow, new GeoLocation { Latitude = 55.7558, Longitude = 37.6173, Name = "Moscow, Russia" } },
        { TestLocations.Dubai, new GeoLocation { Latitude = 25.276987, Longitude = 55.296249, Name = "Dubai, UAE" } },
        { TestLocations.India, new GeoLocation { Latitude = 20.5937, Longitude = 78.9629, Name = "India" } }
    };

    //used in dropdown
    public void SetTestLocation()
    {
        testLocation = (TestLocations)testLocationsDropdown.value;

        if (useTestLocation)
            weatherManager.RefreshSystem();
    }
    
    //used in toggle
    public void ToggleUseLiveLocation()
    {
        useTestLocation = useTestLocationToggle.isOn;
        weatherManager.RefreshSystem();
    }

    public GeoLocation GetLocation()
    {
        if (useTestLocation)
            return testLocations[testLocation];

        return GetIpLocation();
    }

    private GeoLocation GetIpLocation()
    {
        using UnityWebRequest request = UnityWebRequest.Get(ipLookupUrl);
        request.timeout = 10;
        request.SendWebRequest();
        while (!request.isDone) { }

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogWarning("IP lookup failed: " + request.error);
            return null;
        }

        IpLocationResponse data = JsonUtility.FromJson<IpLocationResponse>(request.downloadHandler.text);
        if (!data.success)
        {
            Debug.LogWarning("IP lookup failed: " + data.message);
            return null;
        }

        string locationName = data.country;
        if (!string.IsNullOrEmpty(data.city))
            locationName = data.city + ", " + data.country;

        return new GeoLocation { Latitude = data.latitude, Longitude = data.longitude, Name = locationName };
    }
}

public enum TestLocations
{
    Tokyo,
    Singapore,
    Moscow,
    Dubai,
    India
}
