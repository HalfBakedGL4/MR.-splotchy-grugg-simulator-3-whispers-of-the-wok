using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class S_ApplicationManager : NetworkBehaviour
{
    public static S_ApplicationManager Instance { get; private set; }

    private List<IToggle> _toggles = new List<IToggle>();

    private void Awake()
    {
        Instance = this;
    }

    public void RegisterToggle(IToggle toggle)
    {
        if (!_toggles.Contains(toggle))
            _toggles.Add(toggle);
        print("Registered, " + toggle);
    }

    [Networked] private bool isActive { get; set; }
    public void Toggle()
    {
        isActive = !isActive;

        if (isActive)   EnableApplications();
        
        else DisableApplications();
    }

    [ContextMenu("Disable Applications")]
    private void DisableApplications()
    {
        foreach (var toggle in _toggles)
        {
            toggle.SetApplicationActive(false);
        }
    }

    [ContextMenu("Enable Applications")]
    private void EnableApplications()
    {
        foreach (var toggle in _toggles)
        {
            toggle.SetApplicationActive(true);
        }
    }
}