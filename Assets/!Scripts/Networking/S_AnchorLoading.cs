using System.Collections.Generic;
using UnityEngine;
using Meta.XR; // Import Meta's XR SDK
using System;
using static OVRSpatialAnchor;

public class S_AnchorLoading : MonoBehaviour
{
    public Guid groupUuid;  // The unique Group UUID
    public List<UnboundAnchor> unboundAnchors = new List<UnboundAnchor>(); // List to hold unbound anchors

    // Function to load anchors shared with the group
    public void LoadSharedAnchors()
    {
        OVRTask<OVRResult<List<UnboundAnchor>, OperationResult>> loadTask = OVRSpatialAnchor.LoadUnboundSharedAnchorsAsync(groupUuid, unboundAnchors);

        loadTask.ContinueWith((task) =>
        {
            if (task.Success)
            {
                Debug.Log("Shared anchors loaded successfully!");

                // Localize each anchor after loading
                foreach (var anchor in task.Value)
                {
                    LocalizeAnchor(anchor);
                }
            } else
            {
                Debug.LogError("Failed to load anchors");
            }
        });

    }

    // Function to localize an unbound anchor
    private void LocalizeAnchor(UnboundAnchor anchor)
    {
        // Bind the anchor to the real-world position (localize it)
        anchor.LocalizeAsync();
    }

}
