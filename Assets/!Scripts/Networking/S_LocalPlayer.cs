using Fusion;
using Fusion.Sockets;
using System;
using System.Collections.Generic;
using UnityEngine;


public enum Buttons
{
    trigger,
    grip
}
public enum RigPart
{
    None,
    Headset,
    LeftController,
    RightController,
    Undefined
}

[Serializable]
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

    public Buttons buttons;
}

public class S_LocalPlayer : MonoBehaviour, INetworkRunnerCallbacks
{
    [SerializeField] RigInput input;
    public S_HardwarePart head, rightHand, leftHand;

    [SerializeField]NetworkRunner runner;

    private void OnDestroy()
    {
        runner.RemoveCallbacks(this);
        Debug.Log("disconnect");
    }

    private void Start()
    {
        runner = FindFirstObjectByType<NetworkRunner>();
        runner.AddCallbacks(this);
        Debug.Log("connect");

        //transform.position = new Vector3(UnityEngine.Random.Range(-5, 5), 0, UnityEngine.Random.Range(-5, 5));
    }

    private void Update()
    {
        if (runner == null) return;
        name = runner.LocalPlayer.ToString();
    }

    public void InpitTest(NetworkRunner runner, NetworkInput netInput)
    {
        Debug.Log("local");
    }

    public void OnInput(NetworkRunner runner, NetworkInput netInput)
    {
        Debug.Log("Input");

        this.input.playAreaPosition = transform.position;
        this.input.playAreaRotation = transform.rotation;

        this.input.leftHandPosition = leftHand.transform.position;
        this.input.leftHandRotation = leftHand.transform.rotation;
        this.input.rightHandPosition = rightHand.transform.position;
        this.input.rightHandRotation = rightHand.transform.rotation;
        this.input.headPosition = head.transform.position;
        this.input.headRotation = head.transform.rotation;

        netInput.Set(input);
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
