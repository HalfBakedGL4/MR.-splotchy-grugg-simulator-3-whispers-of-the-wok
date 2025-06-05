using UnityEngine;
using Fusion;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using System.Collections;
using System.Linq;
using static Unity.Collections.Unicode;
using UnityEngine.UIElements;

public class ColocationManager : NetworkBehaviour
{
    [SerializeField] private AlignmentManager alignmentManager;
    [SerializeField] private ColocationInputHandler colocationInputHandler;

    private Guid _sharedAnchorGroupId;
    private bool _isAdvertising = false;
    private bool _isCreatingAnchor = false;
    public override void Spawned()
    {
        base.Spawned();
        Runner.Spawn(colocationInputHandler, Vector3.zero, Quaternion.identity, Runner.LocalPlayer);
        PrepareColocation();
    }

    private void PrepareColocation()
    {
        if (Object.HasStateAuthority)
        {
            Debug.Log("Colocation: Starting advertisement...");
            AdvertiseColocationSession();
        }
        else
        {
            Debug.Log("Colocation: Starting discovery...");
            DiscoverNearBySession();
        }
    }

    private async void AdvertiseColocationSession()
    {
        if (_isAdvertising)
        {
            Debug.Log("Colocation: Already advertising. Skipping... Creating a new Alignment Anchor");
            CreateAndShareAlignmentAnchor();
            return;
        }

        try
        {
            var advertisementData = Encoding.UTF8.GetBytes("SharedSpatialAnchorSession");
            var startAdvertisementResult = await OVRColocationSession.StartAdvertisementAsync(advertisementData);

            if (startAdvertisementResult.Success)
            {
                _isAdvertising = true;
                _sharedAnchorGroupId = startAdvertisementResult.Value;
                Debug.Log("Colocation: ADvertisement started successfully. UUID: " + _sharedAnchorGroupId);
                CreateAndShareAlignmentAnchor();
            }
            else
            {
                Debug.LogError("Colocation failed with status: " + startAdvertisementResult.Status);
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Colocation failed error: " + e.Message);
        }
    }

    private async void DiscoverNearBySession()
    {
        try
        {
            OVRColocationSession.ColocationSessionDiscovered += OnColocationSessionDiscovered;

            var discoveryResult = await OVRColocationSession.StartDiscoveryAsync();
            if (!discoveryResult.Success)
            {
                Debug.LogError("Colocation: Discovery failed with status: " + discoveryResult.Status);
                return;
            }

            Debug.Log("Colocation: Discovery started successfully.");
        }
        catch (Exception e)
        {
            Debug.LogError("Colocation: Error during session: " + e.Message);
        }
    }
    private void OnColocationSessionDiscovered(OVRColocationSession.Data session)
    {
        OVRColocationSession.ColocationSessionDiscovered -= OnColocationSessionDiscovered;

        _sharedAnchorGroupId = session.AdvertisementUuid;
        Debug.Log("Colocation: Discovered session with UUID: " + _sharedAnchorGroupId);
        LoadAndAlignToAnchor(_sharedAnchorGroupId);
    }

    private async void CreateAndShareAlignmentAnchor()
    {
        if (_isCreatingAnchor) return;
        _isCreatingAnchor = true;

        await DestroyAnchors();
        try
        {
            Debug.Log("Colocation: Creating aligment anchor...");
            var anchor = await CreateAnchor(Vector3.zero, Quaternion.identity);

            if (anchor == null)
            {
                Debug.LogError("Colocation: Failed to create alignment anchor.");
                return;
            }

            if (!anchor.Localized)
            {
                Debug.LogError("Colocation: Anchor is not localized. Cannot proceed with starting.");
                return;
            }

            var saveResult = await anchor.SaveAnchorAsync();
            if (!saveResult.Success)
            {
                Debug.LogError("Colocation: Failed to save alignment anchor. Error: " + saveResult);
                return;
            }

            Debug.Log("Colocation: Alignment anchor saved successfully. UUID: " + anchor.Uuid);

            var shareResult = await OVRSpatialAnchor.ShareAsync(new List<OVRSpatialAnchor> { anchor }, _sharedAnchorGroupId);

            if (!shareResult.Success)
            {
                Debug.LogError("Colocation: Failed to share alignment anchor. Error: " + anchor.Uuid);
                return;
            }

            Debug.Log("Colocation: Alignment anchor shared successfully. Group Uuid: " + _sharedAnchorGroupId);
        }
        catch (Exception e)
        {
            Debug.LogError("Colocation: Error during anchor creation and sharing: " + e.Message);
        }
        finally
        {
            _isCreatingAnchor = false;
        }
    }

    private async Task<OVRSpatialAnchor> CreateAnchor(Vector3 position, Quaternion rotation)
    {
        try
        {
            var anchorGameObject = new GameObject("Alignment Anchor")
            {
                transform =
                {
                    position = position,
                    rotation = rotation
                }
            };

            var spatialAnchor = anchorGameObject.AddComponent<OVRSpatialAnchor>();
            while (!spatialAnchor.Created)
            {
                await Task.Yield();
            }

            Debug.Log("Colocation: Anchor created successfully, UUID: " + spatialAnchor.Uuid);
            return spatialAnchor;
        }
        catch (Exception e)
        {
            Debug.LogError("Colocation: Error during anchor creation: " + e.Message);
            return null;
        }
    }

    private async Task DestroyAnchors()
    {
        var anchors = FindObjectsByType<OVRSpatialAnchor>(FindObjectsSortMode.None);
        await EraseAnchors(anchors);
        foreach (var anchor in anchors)
        {
            if (anchor != null)
            {
                Debug.Log("Colocation: Destroying anchor: " + anchor.Uuid);
                Destroy(anchor.gameObject);
            }
        }
        Debug.Log("Colocation: All anchors destroyed and advertising stopped.");
    }

    private async Task EraseAnchors(IEnumerable<OVRSpatialAnchor> anchors)
    {
        var result = await OVRSpatialAnchor.EraseAnchorsAsync(anchors, null);
        if (result.Success)
        {
            Debug.Log($"Colocation: Successfully erased anchors.");
        }
        else
        {
            Debug.LogError($"Colocation: Failed to erase anchors {anchors.Count()} with result {result.Status}");
        }
    }

    private async void LoadAndAlignToAnchor(Guid groupUuid)
    {
        try
        {
            Debug.Log("Colocation: Loading anchors for Group UUID: " + groupUuid);

            var unboundAnchors = new List<OVRSpatialAnchor.UnboundAnchor>();
            var loadResult = await OVRSpatialAnchor.LoadUnboundSharedAnchorsAsync(groupUuid, unboundAnchors);

            if (!loadResult.Success || unboundAnchors.Count == 0)
            {
                Debug.LogError("Colocation: failed to load anchors. Success: " + loadResult.Success + ", Count: " + unboundAnchors.Count);
                return;
            }

            foreach (var unboundAnchor in unboundAnchors)
            {
                if (await unboundAnchor.LocalizeAsync())
                {
                    Debug.Log("Colocation: Anchor localized successfully, UUID: " + unboundAnchor.Uuid);

                    var anchorGameObject = new GameObject("Anchor_" + unboundAnchor.Uuid);
                    var spatialAnchor = anchorGameObject.AddComponent<OVRSpatialAnchor>();
                    unboundAnchor.BindTo(spatialAnchor);

                    alignmentManager.AlignUserToAnchor(spatialAnchor);
                    return;
                }

                Debug.LogWarning("Colocation: Failed to localize anchor: " + unboundAnchor.Uuid);
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Colocation: Failed loading and localizing anchors: " + e.Message);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void RpcResetColocation()
    {
        Debug.Log("Colocation: Resetting for all users...");
        PrepareColocation();
    }

    public void ResetColocationForAll()
    {
        RpcResetColocation();
    }
    
}