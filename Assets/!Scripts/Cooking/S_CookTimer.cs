using System;
using UnityEngine;

public class S_CookTimer : MonoBehaviour
{
    private float _underCookedTime;
    private float _perfectlyCookedTime;
    private float _overCookedTime;
    
    private bool _isCooking;
    

    [SerializeField] private GameObject timerDial;
    [SerializeField] private float rotationSpeed;
    
    public void SetAllTimers(float underCookedTime, float perfectlyCookedTime, float overCookedTime)
    {
        _underCookedTime = underCookedTime;
        _perfectlyCookedTime = perfectlyCookedTime;
        _overCookedTime = overCookedTime;
    }

    public void UpdateTimer(float currentTime)
    {
        float totalTime = _overCookedTime;
        float totalDegrees = 155f;

        float segment1End = _underCookedTime;        // Ends at 80°
        float segment2End = _perfectlyCookedTime;    // Ends at 113°
        float segment3End = _overCookedTime;         // Ends at 135°
        float burntEnd = _overCookedTime + 3f;      // Ends at 155° (optional extra buffer)

        float targetAngle = 0f;

        if (currentTime <= segment1End)
        {
            // 0° to 80°
            float t = currentTime / segment1End;
            targetAngle = Mathf.Lerp(0f, 80f, t);
        }
        else if (currentTime <= segment2End)
        {
            // 80° to 113°
            float t = (currentTime - segment1End) / (segment2End - segment1End);
            targetAngle = Mathf.Lerp(80f, 113f, t);
        }
        else if (currentTime <= segment3End)
        {
            // 113° to 135°
            float t = (currentTime - segment2End) / (segment3End - segment2End);
            targetAngle = Mathf.Lerp(113f, 135f, t);
        }
        else
        {
            // 135° to 155° (burnt)
            float t = Mathf.Clamp01((currentTime - segment3End) / (burntEnd - segment3End));
            targetAngle = Mathf.Lerp(135f, 155f, t);
        }
        if (!_isCooking)
            targetAngle = 0.0f;

        // Smoothly animate toward the target rotation
        float currentY = timerDial.transform.localEulerAngles.y;
        float smoothAngle = Mathf.LerpAngle(currentY, targetAngle, rotationSpeed * Time.deltaTime);

        timerDial.transform.localEulerAngles = new Vector3(0, smoothAngle, 0);
    }


    public void TimerToggle(bool toggle)
    {
        _isCooking = toggle;
    }
}
