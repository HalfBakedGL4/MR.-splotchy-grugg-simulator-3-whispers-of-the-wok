using Fusion;
using NaughtyAttributes;
using System.Collections;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public enum Planet
{
    CyberPlanet = -1,
    Pluto = 0,
    Saturn = 1
}

public class S_SettingsMenu : MonoBehaviour
{
    public static S_SettingsMenu instance;

    public AnimationCurve xAxis;

    [SerializeField] Planet currentPlanet;
    public SerializableDictionary<Planet, GameObject> planets;

    static Vector3 defaultPos { get; } = new Vector3(0, 0.6f, 1.5f);
    static Vector3 defaultScale { get; } = new Vector3(0, 0, 0);
    static Vector3 displayPos { get; } = new Vector3(0, 0.6f, 0.5f);
    static Vector3 displayScale { get; } = new Vector3(2, 2, 2);

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
        if(instance.planets[currentPlanet] != null)
            instance.planets[currentPlanet].transform.Rotate(0, 10 * Time.deltaTime, 0);
    }

    public IEnumerator UpdateSelectedPlanet(Planet planet)
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
                MoveForwards(current, t);
            }

            if(previous != null)
            {
                MoveBack(previous, t);
            }
        }
        if (previous != null)
            previous.SetActive(false);
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
