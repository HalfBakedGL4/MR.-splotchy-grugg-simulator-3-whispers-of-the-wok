using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class S_FindWallToBreak : MonoBehaviour
{
    [SerializeField] private S_FindPointsOnWalls findPointsOnWalls;
    [SerializeField] private S_PlaceHoleInWall placeHoleInWall;
    private (ARPlane wall, Vector3 pointOnWall) wallPoint;
    
    
    private void Start()
    {
        findPointsOnWalls = FindAnyObjectByType<S_FindPointsOnWalls>();
        placeHoleInWall = FindAnyObjectByType<S_PlaceHoleInWall>();
       
        if (!findPointsOnWalls)
            Debug.LogError("No S_FindPointsOnWalls component found");
        if (!placeHoleInWall)
            Debug.LogError("No S_PlaceHoleInWall component found");
    }
    
    public void FindWallToBreak()
    {
        // Gets Tuple (AR Plane: wall, Vector 3: point)
        wallPoint = findPointsOnWalls.GetRandomWallAndPoint();
    }

    public void MakeHoleInWall()
    {
        placeHoleInWall.SpawnHoleInWall(wallPoint);
    }
}
