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
    }

    private void DisableApplications()
    {
        foreach (var toggle in _toggles)
        {
            toggle.SetApplicationActive(false);
        }
    }

    private void EnableApplications()
    {
        foreach (var toggle in _toggles)
        {
            toggle.SetApplicationActive(true);
        }
    }
}