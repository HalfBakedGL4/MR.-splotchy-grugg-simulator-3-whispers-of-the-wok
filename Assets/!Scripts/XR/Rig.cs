using UnityEngine;

public struct Rig
{
    public S_HardwarePart Body, Head, RightHand, LeftHand;

    public void SetRig(S_LocalPlayer player)
    {
        Head = player.head;
        RightHand = player.rightHand;
        LeftHand = player.leftHand;
    }
}
