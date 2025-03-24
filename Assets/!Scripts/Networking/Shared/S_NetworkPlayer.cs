using Fusion;
using UnityEngine;

namespace Networking.Shared
{
    /// <summary>
    /// Used for updating the networkplayer in shared mode
    /// </summary>
    public class S_NetworkPlayer : NetworkBehaviour
    {
        [Header("References")]
        [SerializeField] Transform leftHand;
        [SerializeField] Transform rightHand; 
        [SerializeField] Transform head;

        S_LocalPlayer localPlayer;
        bool isLocal => Object && Object.HasStateAuthority;



        public override void Spawned()
        {
            base.Spawned();

            if (!isLocal) return;

            localPlayer = transform.parent.GetComponentInChildren<S_LocalPlayer>();
        }

        public override void FixedUpdateNetwork()
        {
            base.FixedUpdateNetwork();

            if (!isLocal || localPlayer == null) return;

            UpdateParts(localPlayer.rigState);
        }
        public override void Render()
        {
            base.Render();

            if (!isLocal) return;
            UpdateParts(localPlayer.rigState);
        }

        void UpdateParts(RigState rig)
        {
            transform.position = rig.playAreaPosition;
            transform.rotation = rig.playAreaRotation;

            leftHand.position = rig.leftHandPosition;
            leftHand.rotation = rig.leftHandRotation;

            rightHand.position = rig.rightHandPosition;
            rightHand.rotation = rig.rightHandRotation;

            head.position = rig.headPosition;
            head.rotation = rig.headRotation;
        }
    }
}
