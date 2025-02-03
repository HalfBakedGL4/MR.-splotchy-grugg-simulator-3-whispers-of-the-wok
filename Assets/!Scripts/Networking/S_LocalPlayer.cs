using UnityEngine;
using Fusion;

public class S_LocalPlayer : MonoBehaviour
{
    public Rig rig;

    [SerializeField] Transform Head, RightHand, LeftHand;

    [SerializeField] NetworkRunner runner;

    private void Update()
    {
        name = runner.LocalPlayer.ToString();
    }

    void FixedUpdate()
    {
        rig.SetRig(transform, Head, RightHand, LeftHand);
    }
}
