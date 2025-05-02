using Fusion;
using UnityEngine;
using UnityEngine.InputSystem;

public class ColocationInputHandler : NetworkBehaviour
{
    public ColocationManager colocationManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        colocationManager = FindFirstObjectByType<ColocationManager>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void clickA()
    {
        Debug.Log("Colocation: A button pressed by player " + Object.InputAuthority);
        RPC_RequestResetColocation();
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    private void RPC_RequestResetColocation(RpcInfo info = default)
    {
        if (colocationManager != null)
        {
            Debug.Log("Colocation: Host recieved reset request from " + info.Source);
            colocationManager.ResetColocationForAll();
        }
    }
}
