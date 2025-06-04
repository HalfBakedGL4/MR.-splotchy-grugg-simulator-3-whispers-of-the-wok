using NaughtyAttributes;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public enum Planet
{
    CyberPlanet = -1,
    None = 0,
    Saturn = 1
}

public class S_SettingsMenu : MonoBehaviour
{
    public static S_SettingsMenu instance;

    [SerializeField] Planet _currentPlanet;
    public static Planet currentPlanet = Planet.None;
    public SerializableDictionary<Planet, GameObject> planets;

    static Vector3 defaultPos { get; } = new Vector3(0, 0.6f, 1.5f);
    static Vector3 defaultScale { get; } = new Vector3(0, 0, 0);
    static Vector3 displayPos { get; } = new Vector3(0, 0.6f, 0.5f);
    static Vector3 displayScale { get; } = new Vector3(2, 2, 2);

    private void Awake()
    {
        instance = this;
    }
    private void Update()
    {
        _currentPlanet = currentPlanet;
    }

    public static IEnumerator UpdateSelectedPlanet(Planet planet)
    {
        GameObject current = instance.planets[planet];
        GameObject previous = instance.planets[currentPlanet];

        Debug.Log(current + " + " + previous);

        currentPlanet = planet;

        if(current != null)
            current.SetActive(true);

        float t = 0;

        while(t < 1)
        {
            yield return new WaitForEndOfFrame();

            t += Time.deltaTime;

            Debug.Log(t < 1);

            if(current != null)
            {
                current.transform.localPosition = Vector3.Lerp(defaultPos, displayPos, t);
                current.transform.localScale = Vector3.Lerp(defaultScale, displayScale, t);
            }

            if(previous != null)
            {
                previous.transform.localPosition = Vector3.Lerp(displayPos, defaultPos, t);
                previous.transform.localScale = Vector3.Lerp(displayScale, defaultScale, t);
            }
        }
        if (previous != null)
            previous.SetActive(false);
    }
}
