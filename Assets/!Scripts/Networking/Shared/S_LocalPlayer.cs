using Fusion;
using System;
using UnityEngine;

namespace Networking.Shared
{
    [Serializable]
    public struct RigState
    {
        public Vector3 playAreaPosition;
        public Quaternion playAreaRotation;

        public Vector3 leftHandPosition;
        public Quaternion leftHandRotation;

        public Vector3 rightHandPosition;
        public Quaternion rightHandRotation;

        public Vector3 headPosition;
        public Quaternion headRotation;
    }

    /// <summary>
    /// Used for sending infromation about the local player in shared mode
    /// </summary>
    public class S_LocalPlayer : MonoBehaviour
    {
        [SerializeField] Transform leftHand, rightHand, head;

        RigState rig = default;

        public virtual RigState rigState
        {
            get
            {
                rig.playAreaPosition = transform.position;
                rig.playAreaRotation = transform.rotation;

                rig.leftHandPosition = leftHand.position;
                rig.leftHandRotation = leftHand.rotation;

                rig.rightHandPosition = rightHand.position;
                rig.rightHandRotation = rightHand.rotation;

                rig.headPosition = head.position;
                rig.headRotation = head.rotation;

                return rig;
            }
        }
    }

}
