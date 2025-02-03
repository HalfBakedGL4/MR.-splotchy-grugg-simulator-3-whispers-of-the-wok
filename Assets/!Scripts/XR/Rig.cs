using UnityEngine;

public struct Rig
{
    public Transform Body, Head, RightHand, LeftHand;

    public void SetRig(Transform body, Transform head, Transform rightHand, Transform leftHand)
    {
        Body = body;
        Head = head;
        RightHand = rightHand;
        LeftHand = leftHand;
    }
}
