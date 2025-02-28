using UnityEngine;
using UnityEngine.UI;

public class FloatingHealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;

    public void UpdateHealthBar(float currentValue, float maxValue) 
    {
        Debug.Log("RUNNNING HEALTH");
        slider.maxValue = maxValue;
        slider.value = currentValue;
    }

    void Update() 
    {
        
    }

}
