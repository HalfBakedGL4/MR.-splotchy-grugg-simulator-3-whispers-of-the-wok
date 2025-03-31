using UnityEngine;
using Fusion;

namespace Networking.Shared
{
    public class S_DontDisplayForOwner : NetworkBehaviour
    {
        bool isLocal => Object && Object.HasStateAuthority;

        public override void Spawned()
        {
            base.Spawned();

            if (!TryGetComponent(out MeshRenderer mesh)) return;

            if (isLocal)
            {
                Debug.Log("wont display");
                mesh.enabled = false;
            }
        }
    }
}
