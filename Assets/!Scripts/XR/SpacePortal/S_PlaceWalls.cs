using System;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class S_PlaceWalls : MonoBehaviour
{
    [SerializeField] private GameObject wallPrefab;
    [SerializeField] private float wallPrefabSize;
    [SerializeField] private float spaceFromWall = 0.1f;




    public void SetupWall(ARTrackablesChangedEventArgs<ARPlane> changes)
    {
        foreach (ARPlane plane in changes.added)
        {
            if (PlaneClassifications.WallFace == plane.classifications)
            {
                // Finds number of rows and columns to create a grid to place items
                var rows = plane.size.x / wallPrefabSize;
                var cols = plane.size.y / wallPrefabSize;

                rows = Mathf.CeilToInt(rows);
                cols = Mathf.CeilToInt(cols);

                

                //debugText.text += item.name + ": " + item.size + " Rows: " + rows + " Cols: " + cols + "\n";

                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < cols; j++)
                    {
                        // Offset to place items in middle of grid square
                        var offsetVector = new Vector3(wallPrefabSize * (i + 0.5f), spaceFromWall, wallPrefabSize * (j + 0.5f)) -
                                           new Vector3(plane.extents.x, 0, plane.extents.y);
                        // Spawn Object
                        var objectInstance = Instantiate(wallPrefab, plane.transform.position, plane.transform.rotation);
                        // Make Object Child of ARPlane to place it through localTransform
                        objectInstance.transform.parent = plane.transform;
                        objectInstance.transform.localPosition += offsetVector;
                    }
                }
            
            }
        }
    }
}
