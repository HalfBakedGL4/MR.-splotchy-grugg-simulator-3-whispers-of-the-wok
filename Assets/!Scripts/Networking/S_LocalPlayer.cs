using UnityEngine;
using Fusion;
using Fusion.Sockets;
using System.Collections.Generic;
using UnityEngine.Windows;
using static Unity.Collections.Unicode;

public enum RigPart
{
    None,
    Headset,
    LeftController,
    RightController,
    Undefined
}

public struct RigInput : INetworkInput
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

public class S_LocalPlayer : MonoBehaviour, INetworkRunnerCallbacks
{
    public S_HardwarePart head, rightHand, leftHand;

    [SerializeField] NetworkRunner runner;

    private void Start()
    {
        transform.position = new Vector3(Random.Range(-5, 5), 0, Random.Range(-5, 5));
        runner.AddCallbacks(this);
    }

    private void Update()
    {
        name = runner.LocalPlayer.ToString();
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        Debug.Log("Input");
        RigInput rigInput = new RigInput();
        rigInput.playAreaPosition = transform.position;
        rigInput.playAreaRotation = transform.rotation;

        rigInput.leftHandPosition = leftHand.transform.position;
        rigInput.leftHandRotation = leftHand.transform.rotation;
        rigInput.rightHandPosition = rightHand.transform.position;
        rigInput.rightHandRotation = rightHand.transform.rotation;
        rigInput.headPosition = head.transform.position;
        rigInput.headRotation = head.transform.rotation;

        input.Set(rigInput);
    }
    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
    }

    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
    }

    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {   
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, System.ArraySegment<byte> data)
    {
    }

    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
    {
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
    }

    public void OnConnectedToServer(NetworkRunner runner)
    {
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
    }
}
