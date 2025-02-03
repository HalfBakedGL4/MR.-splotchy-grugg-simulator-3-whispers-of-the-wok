using UnityEngine;
using Fusion;

public class S_LocalPlayer : MonoBehaviour
{
    public S_HardwarePart Head, RightHand, LeftHand;

    [SerializeField] NetworkRunner runner;

    private void Update()
    {
        name = runner.LocalPlayer.ToString();
    }
}
