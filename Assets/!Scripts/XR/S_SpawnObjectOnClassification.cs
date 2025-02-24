using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using Random = UnityEngine.Random;

public class S_SpawnObjectOnClassification : MonoBehaviour
{
    [SerializeField] private ARPlaneManager planeManager;
    [SerializeField] private PlaneClassifications classifications;
    [SerializeField] private GameObject[] spawnObjects;
    [SerializeField] private TMP_Text debugText;
    
    private float basketSize = 0.5f;
    private void OnEnable()
    {
        planeManager.planesChanged += PlaceObjectOnPlane;
    }
    private void OnDisable()
    {
        planeManager.planesChanged -= PlaceObjectOnPlane;
    }

    private void PlaceObjectOnPlane(ARPlanesChangedEventArgs obj)
    
    {
        List<ARPlane> newPlane = obj.added;
        foreach (var item in newPlane)
        {
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
                        var objectInstance = Instantiate(spawnObjects[Random.Range(0,spawnObjects.Length)], item.transform.position, Quaternion.identity);
                        // Make Object Child of ARPlane to place it through localTransform
                        objectInstance.transform.parent = item.transform;
                        objectInstance.transform.localPosition += offsetVector;
                        // Where to spawn object in world Space
                        //debugText.text += objectInstance.name + ": " + objectInstance.transform.localPosition + "\n";
                    }
                }
            }
        }
    }
}
