using System.Collections.Generic;
using Fusion;
using TMPro;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using Random = UnityEngine.Random;

public class S_FindPointsOnWalls : MonoBehaviour
{
    [SerializeField] private float holeSize;
    
    private Dictionary<ARPlane, List<Vector3>> wallPoints = new ();
    
    private ARPlaneManager planeManager;

    private void OnEnable()
    {
        planeManager = FindFirstObjectByType<ARPlaneManager>();
        if (planeManager == null)
            Log.Error("There are no plane manager.");
        planeManager.trackablesChanged.AddListener(FindWallPoints);
    }

  
    [SerializeField] private TMP_Text debugText;

    // Must be connected to an AR plane manager
    void FindWallPoints(ARTrackablesChangedEventArgs<ARPlane> changes)
    {
        foreach (ARPlane plane in changes.added)
        {
            if (PlaneClassifications.WallFace == plane.classifications)
            {
                // Finds number of rows and columns to create a grid to place items
                var rows = plane.size.x / holeSize;
                var cols = plane.size.y / holeSize;
                
                rows = Mathf.CeilToInt(rows);
                cols = Mathf.CeilToInt(cols);

                var rowSize = plane.size.x / rows;
                var colSize = plane.size.y / cols;


                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < cols; j++)
                    {
                        // Offset to place items in middle of grid square
                        var offsetVector = new Vector3(rowSize * (i + 0.5f), 0.1f, colSize * (j + 0.5f)) -
                                           new Vector3(plane.extents.x, 0, plane.extents.y);
                        
                        AddWallPoint(plane, offsetVector);
                    }
                }
                
            }
        }
    }

    private void AddWallPoint(ARPlane plane, Vector3 point)
    {
        if (!wallPoints.ContainsKey(plane))
        {
            wallPoints[plane] = new List<Vector3>();
        }

        debugText.text += "."; 
        wallPoints[plane].Add(point);
    }
    

    // Returns Tuple containing the wall and point on wall
    // A Couple of line to debug and remove used spots so to not spawn hole on same position
    public (ARPlane, Vector3) GetRandomWallAndPoint()
    {
        // Check if dictionary is empty
        if (wallPoints == null || wallPoints.Count == 0)
        {
            Debug.LogWarning("No walls available in the dictionary.");
            debugText.text += "No walls available in the dictionary";
            return (null, Vector3.zero);
        }

        // Convert dictionary keys (ARPlanes) to a list
        List<ARPlane> walls = new List<ARPlane>(wallPoints.Keys);

        // Check if the walls list is empty
        if (walls.Count == 0)
        {
            Debug.LogWarning("Walls list is empty.");
            debugText.text += "Walls list is empty.";

            return (null, Vector3.zero);
        }

        // Get random wall
        ARPlane randomWall = walls[Random.Range(0, walls.Count)];

        // Ensure the selected wall has points
        if (!wallPoints.ContainsKey(randomWall) || wallPoints[randomWall] == null || wallPoints[randomWall].Count == 0)
        {
            Debug.LogWarning($"Selected wall '{randomWall.name}' has no points.");
            debugText.text += $"Selected wall '{randomWall.name}' has no points.";
            return (randomWall, Vector3.zero);
        }

        // Get a random point on the selected wall
        List<Vector3> points = wallPoints[randomWall];
        int randomIndex = Random.Range(0, points.Count);
        Vector3 randomPoint = points[randomIndex];

        // Remove the point from the list
        points.RemoveAt(randomIndex);

        // If the list is now empty, remove the wall from the dictionary
        if (points.Count == 0)
        {
            wallPoints.Remove(randomWall);
            Debug.Log($"Wall '{randomWall.name}' removed from dictionary as it has no more points.");
        }

        return (randomWall, randomPoint);
    }
    private void OnDisable()
    {
        planeManager.trackablesChanged.RemoveListener(FindWallPoints);

    }
}
