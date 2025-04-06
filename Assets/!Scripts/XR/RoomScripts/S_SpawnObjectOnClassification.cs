using System;
using System.Collections.Generic;
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
    
    [SerializeField] private S_OrderWindow orderWindow;
    [SerializeField] private float windowHeight = 1.0f;
    private float basketSize = 0.5f;
    
    private bool windowPlaced = false;

    

    public override void Spawned()
    {
        Debug.Log("[Spawn Objects] Network object spawned in Shared Mode.");
        PlaceObjectOnPlane(planeManager.trackables);
    }

    // Needs to be referenced in the editor in ARPlaneManager

    private void OnEnable()
    {
        planeManager = FindFirstObjectByType<ARPlaneManager>();
        planeManager.trackablesChanged.AddListener(PlaceObjectOnPlane);
    }

    // Needs to be referenced in the editor in ARPlaneManager
    public void PlaceObjectOnPlane(ARTrackablesChangedEventArgs<ARPlane> changes)
    {
        if (!islocal) return;

        int i = 0;

        foreach (var item in changes)
        {

            if (item == null)
            {
                Debug.LogError("[Spawn Objects] Null ARPlane in 'added'");
            } else
            {
                GameObject prefabToSpawn = spawnObjects[i];
                Quaternion rotation = Quaternion.identity;
                Runner.Spawn(prefabToSpawn, Vector3.zero, rotation);
                // Place applications on table across the room
                if (item.classifications == PlaneClassifications.Table)
                {
                //    GameObject prefabToSpawn = spawnObjects[i];
                //    var offsetVector = new Vector3(1, item.transform.position.y, 1) -
                //                              new Vector3(item.extents.x, 0, item.extents.y);
                //    Quaternion rotation = Quaternion.identity;
                //    Runner.Spawn(prefabToSpawn, Vector3.zero, rotation);


                    //// Finds number of rows and columns to create a grid to place items
                    //var rows = item.size.x / basketSize;
                    //var cols = item.size.y / basketSize;

                    //rows = Mathf.FloorToInt(rows);
                    //cols = Mathf.FloorToInt(cols);

                    //var rowSize = item.size.x / rows;
                    //var colSize = item.size.y / cols;
                    //Debug.LogWarning("[Spawn Objects] size of item: " + item.name + " with rows: " + rowSize + " and cols: " + colSize);

                    ////debugText.text += item.name + ": " + item.size + " Rows: " + rows + " Cols: " + cols + "\n";

                    //for (int i = 0; i < rows; i++)
                    //{
                    //    for (int j = 0; j < cols; j++)
                    //    {
                    //        Debug.LogWarning("[Spawn Objects] in for - i=" + i + " j=" + j);
                    //        // Offset to place items in middle of grid square
                    //        var offsetVector = new Vector3(rowSize * (i + 0.5f), item.transform.position.y, colSize * (j + 0.5f)) -
                    //                           new Vector3(item.extents.x, 0, item.extents.y);
                    //        // Spawn Object
                    //        Vector3 worldPosition = item.transform.position + item.transform.rotation * offsetVector;
                    //        Quaternion rotation = Quaternion.identity;

                    //        GameObject prefabToSpawn = spawnObjects[Random.Range(0, spawnObjects.Length)];
                    //        if (prefabToSpawn == null)
                    //        {
                    //            Debug.LogError("[Spawn Objects] Prefab To Spawn is Null");
                    //        } else
                    //        {
                    //            Debug.LogWarning("[Spawn Objects] in runner.spawn");
                    //            Runner.Spawn(prefabToSpawn, worldPosition, rotation);
                    //        }
                    //        // Where to spawn object in world Space
                    //        //debugText.text += objectInstance.name + ": " + objectInstance.transform.localPosition + "\n";
                    //    }
                    //}
                }

            if (item.classifications == PlaneClassifications.WallFace && !windowPlaced)
                {
                    //debugText.text = item.transform.rotation.ToString();
                    //debugText.text += "\n";
                    //debugText.text += "Rotation (Euler Angles): " + item.transform.eulerAngles.ToString();

                    var windowInstance = Runner.Spawn(orderWindow, item.transform.position, Quaternion.identity);

                    windowInstance.transform.parent = item.transform;
                    windowInstance.transform.localEulerAngles = new Vector3(180, -90, -90);
                    windowInstance.transform.position = new Vector3(windowInstance.transform.position.x, windowHeight, windowInstance.transform.position.z);

                    windowPlaced = true;
                }

                i++;
            }

            
        }
    }

    private void OnDisable()
    {
        planeManager.trackablesChanged.RemoveListener(PlaceObjectOnPlane);
    }

  
}
