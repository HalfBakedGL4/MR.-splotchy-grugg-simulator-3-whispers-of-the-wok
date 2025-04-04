using Fusion;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

public class S_SpawnObjectOnClassification : NetworkBehaviour
{
    [Header("Link this script to AR Plane Manager!!")]
    [SerializeField] private PlaneClassifications classifications;
    [SerializeField] private GameObject[] spawnObjects;
    [SerializeField] private TMP_Text debugText;

    [SerializeField] private NetworkRunner runner;
    
    [SerializeField] private S_OrderWindow orderWindow;
    [SerializeField] private float windowHeight = 1.0f;
    private float basketSize = 0.5f;
    
    private bool windowPlaced = false;

    
    // Needs to be referenced in the editor in ARPlaneManager
    public void PlaceObjectOnPlane(ARTrackablesChangedEventArgs<ARPlane> changes)
    {
        if (Object.HasStateAuthority) return; // Only host spawns

        foreach (var item in changes.added)
        {
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

                //debugText.text += item.name + ": " + item.size + " Rows: " + rows + " Cols: " + cols + "\n";

                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < cols; j++)
                    {
                        // Offset to place items in middle of grid square
                        var offsetVector = new Vector3(rowSize * (i + 0.5f), 0, colSize * (j + 0.5f)) -
                                           new Vector3(item.extents.x, 0, item.extents.y);
                        // Spawn Object
                        Vector3 worldPosition = item.transform.position + item.transform.rotation * offsetVector;
                        Quaternion rotation = Quaternion.identity;

                        GameObject prefabToSpawn = spawnObjects[Random.Range(0, spawnObjects.Length)];
                        if (prefabToSpawn == null)
                        {
                            Debug.LogError("[Spawn Objects] Prefab To Spawn is Null");
                        } else if (worldPosition == null)
                        {
                            Debug.LogError("[Spawn Objects] World Position is Null");
                        } else if (rotation == null)
                        {
                            Debug.LogError("[Spawn Objects] Rotation is Null");
                        } else
                        {
                            runner.Spawn(prefabToSpawn, worldPosition, rotation);
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
