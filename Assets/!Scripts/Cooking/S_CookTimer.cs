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
        float degreePerSec;
        var speed = rotationSpeed * Time.deltaTime;
        if (_isCooking)
        {
            // Timer on blue (from 0 to 80) Under Cooked
            if (currentTime < _underCookedTime)
            {
                var targetDegree = 80.0f;
                degreePerSec = targetDegree / _underCookedTime;
            }
            // Timer on green (from 80 to 113) Perfectly Cooked
            else if (currentTime < _perfectlyCookedTime)
            {
                var targetDegree = 113.0f;
                degreePerSec = targetDegree / _perfectlyCookedTime;
            }
            // Timer on yellow (from 113 to 135) Overcooked
            else if (currentTime < _overCookedTime)
            {
                var targetDegree = 135.0f;
                degreePerSec = targetDegree / _overCookedTime;
            }
            // Timer on red (from 135 to 155) Burnt
            else
            {
                var targetDegree = 155.0f;
                degreePerSec = targetDegree / _overCookedTime;
            }
        }
        else
        {
            // Timer goes towards 0
            degreePerSec = 0;
        }

        var targetRotation = currentTime * degreePerSec;
        targetRotation = Mathf.Clamp(targetRotation, 0, 155);
        
        var lerpAngle = Mathf.LerpAngle(timerDial.transform.localEulerAngles.y, targetRotation, speed);

        timerDial.transform.localEulerAngles = new Vector3(0, lerpAngle, 0);

    }

    public void TimerToggle(bool toggle)
    {
        _isCooking = toggle;
    }
}
