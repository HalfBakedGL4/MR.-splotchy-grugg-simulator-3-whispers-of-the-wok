using Fusion;
using Fusion.Sockets;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using static Unity.Collections.Unicode;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

public class S_SpawnObjectOnClassification : NetworkBehaviour
{
    [Header("Link this script to AR Plane Manager!!")]
    [SerializeField] private PlaneClassifications classifications;
    [SerializeField] private GameObject[] spawnObjects;
    [SerializeField] private TMP_Text debugText;
    
    [SerializeField] private S_OrderWindow orderWindow;
    [SerializeField] private float windowHeight = 1.0f;
    private float basketSize = 0.5f;
    
    private bool windowPlaced = false;

    bool islocal => Object && Object.HasStateAuthority;
    private bool isFusionConnected = false;

    public override void Spawned()
    {
        Debug.Log("[Spawn Objects] Network object spawned in Shared Mode.");
        isFusionConnected = true;
    }

    // Needs to be referenced in the editor in ARPlaneManager
    public async void PlaceObjectOnPlane(ARTrackablesChangedEventArgs<ARPlane> changes)
    {
        while(!isFusionConnected)
        {
            await Task.Delay(10);
        }

        if (!isFusionConnected)
        {
            Debug.LogWarning("[Spawn Objects] Waiting for Fusion connection...");
            return;
        }

        if (!islocal)
        {
            Debug.LogError("[Spawn Objects] Object doesnt have state authority");
            return;
        }// Only host spawns

        if (changes == null)
        {
            Debug.LogError("[Spawn Objects] Null changes");
        }

        foreach (var item in changes.added)
        {

            if (item == null)
            {
                Debug.LogError("[Spawn Objects] Null ARPlane in 'added'");
                continue;
            }


            // Place applications on table across the room
            if (item.classifications == classifications)
            {
                // Finds number of rows and columns to create a grid to place items
                var rows = item.size.x / basketSize;
                var cols = item.size.y / basketSize;

                rows = Mathf.FloorToInt(rows);
                cols = Mathf.FloorToInt(cols);

                var rowSize = item.size.x / rows;
                var colSize = item.size.y / cols;
                Debug.LogWarning("[Spawn Objects] size of item: " + item.name + " with rows: " + rowSize + " and cols: " + colSize);

                //debugText.text += item.name + ": " + item.size + " Rows: " + rows + " Cols: " + cols + "\n";

                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < cols; j++)
                    {
                        Debug.LogWarning("[Spawn Objects] in for - i=" + i + " j=" + j);
                        // Offset to place items in middle of grid square
                        var offsetVector = new Vector3(rowSize * (i + 0.5f), item.transform.position.y, colSize * (j + 0.5f)) -
                                           new Vector3(item.extents.x, 0, item.extents.y);
                        // Spawn Object
                        Vector3 worldPosition = item.transform.position + item.transform.rotation * offsetVector;
                        Quaternion rotation = Quaternion.identity;

                        GameObject prefabToSpawn = spawnObjects[Random.Range(0, spawnObjects.Length)];
                        if (prefabToSpawn == null)
                        {
                            Debug.LogError("[Spawn Objects] Prefab To Spawn is Null");
                        } else
                        {
                            Debug.LogWarning("[Spawn Objects] in runner.spawn");
                            Runner.Spawn(prefabToSpawn, worldPosition, rotation);
                        }
                        // Where to spawn object in world Space
                        //debugText.text += objectInstance.name + ": " + objectInstance.transform.localPosition + "\n";
                    }
                }
            }

            if (item.classifications == PlaneClassifications.WallFace && !windowPlaced)
            {
                debugText.text = item.transform.rotation.ToString();
                debugText.text += "\n";
                debugText.text += "Rotation (Euler Angles): " + item.transform.eulerAngles.ToString();
                
                var windowInstance = Instantiate(orderWindow, item.transform.position, Quaternion.identity);
                
                windowInstance.transform.parent = item.transform;
                windowInstance.transform.localEulerAngles = new Vector3(180, -90, -90);
                windowInstance.transform.position = new Vector3(windowInstance.transform.position.x, windowHeight, windowInstance.transform.position.z);
                
                windowPlaced = true;
            }
        }
    }

  
}
