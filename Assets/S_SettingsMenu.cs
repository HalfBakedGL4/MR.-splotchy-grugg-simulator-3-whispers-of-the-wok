using Fusion;
using NaughtyAttributes;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public enum Planet
{
    CyberPlanet = -1,
    Pluto = 0,
    Saturn = 1
}

public class S_SettingsMenu : NetworkBehaviour
{
    public static S_SettingsMenu instance;

    public AnimationCurve xAxis;

    [SerializeField, Networked] Planet currentPlanet { get; set; }
    public SerializableDictionary<Planet, GameObject> planets;

    [SerializeField] TMP_Text planetName;

    static Vector3 defaultPos { get; } = new Vector3(0, 0.6f, 1.5f);
    static Vector3 defaultScale { get; } = Vector3.zero;
    static Vector3 displayPos { get; } = new Vector3(0, 0.6f, 0.75f);
    static Vector3 displayScale { get; } = Vector3.one * 1.75f;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        planets[currentPlanet].transform.localPosition = displayPos;
        planets[currentPlanet].transform.localScale = displayScale;
        planets[currentPlanet].SetActive(true);
    }
    private void Update()
    {
        if(planets[currentPlanet] != null)
            planets[currentPlanet].transform.Rotate(0, 10 * Time.deltaTime, 0);

        planetName.text = planets[currentPlanet].name;
    }

    public IEnumerator UpdateSelectedPlanet(Planet planet)
    {
        if (planet != currentPlanet)
        {
            GameObject current = instance.planets[planet];
            GameObject previous = instance.planets[currentPlanet];

            Debug.Log(current + " + " + previous);

            currentPlanet = planet;

            if (current != null)
                current.SetActive(true);

            float t = 0;

            while (t < 1)
            {
                yield return new WaitForEndOfFrame();

                t += Time.deltaTime;

                Debug.Log(t < 1);

                if (current != null)
                {
                    MoveForwards(current, t);
                }

                if (previous != null)
                {
                    MoveBack(previous, t);
                }
            }
            if (previous != null)
                previous.SetActive(false);
        }
    }

    void MoveForwards(GameObject current, float t)
    {
        current.transform.localPosition = Vector3.Lerp(defaultPos, displayPos, t);
        current.transform.localScale = Vector3.Lerp(defaultScale, displayScale, t);
        current.transform.localPosition += new Vector3(instance.xAxis.Evaluate(t), 0, 0);

        current.transform.Rotate(0, 30 * Time.deltaTime, 0);
    }
    void MoveBack(GameObject previous, float t)
    {
        previous.transform.localPosition = Vector3.Lerp(displayPos, defaultPos, t);
        previous.transform.localScale = Vector3.Lerp(displayScale, defaultScale, t);
        previous.transform.localPosition += new Vector3(-instance.xAxis.Evaluate(t), 0, 0);

        previous.transform.Rotate(0, 30 * Time.deltaTime, 0);
    }
}
