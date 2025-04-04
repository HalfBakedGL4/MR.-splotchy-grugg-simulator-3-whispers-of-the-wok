using UnityEngine;
using Fusion;

namespace Networking.Shared
{
    public class S_DontDisplayForOwner : NetworkBehaviour
    {
        [SerializeField] GameObject art;
        bool isLocal => Object && Object.HasStateAuthority;

        public override void Spawned()
        {
            base.Spawned();

            if (isLocal)
            {
                Debug.Log("wont display");
                art.SetActive(false);
            }
        }
    }
}
