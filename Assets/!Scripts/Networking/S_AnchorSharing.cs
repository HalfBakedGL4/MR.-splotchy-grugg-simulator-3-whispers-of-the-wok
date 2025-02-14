using UnityEngine;
using Meta.XR;
using System;
using System.Collections.Generic;
using static OVRSpatialAnchor;
using Fusion;

public class S_AnchorSharing : MonoBehaviour
{
    public Guid groupUuid;  // The unique Group UUID
    public List<OVRSpatialAnchor> anchorsToShare; // List of anchors to be shared
    public NetworkRunner runner;

    // Function to share anchors with the group
    public void ShareAnchors()
    {
        runner = FindFirstObjectByType<NetworkRunner>();
        if (runner.IsServer)
        {
            // Share the anchors asynchronously
            OVRTask<OVRResult<OVRAnchor.ShareResult>> shareTask = OVRSpatialAnchor.ShareAsync(anchorsToShare, groupUuid);

            // Use ContinueWith to handle completion
            shareTask.ContinueWith((task) =>
            {

                if (task.Success)
                {
                    Debug.Log("Anchors shared successfully!");
                }
                else
                {
                    Debug.LogError("Failed to share anchors");
                }
            });
        }
    }
}
