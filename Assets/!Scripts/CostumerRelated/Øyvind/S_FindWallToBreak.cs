using TMPro;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class S_FindWallToBreak : MonoBehaviour
{
    [SerializeField] private S_FindPointsOnWalls findPointsOnWalls;
    [SerializeField] private HoleSpawner holeSpawner;
    private (ARPlane wall, Vector3 pointOnWall) wallPoint;

    
    [SerializeField] private TMP_Text debugText;


    private void Awake()
    {
        if (findPointsOnWalls == null)
            Debug.LogError("No S_FindPointsOnWalls component found");
        if (holeSpawner == null)
            Debug.LogError("No HoleSpawner component found");
    }

    public void Debuggings()
    {
        debugText.text += " Making ";
        MakeHoleInWall();
        debugText.text += " hole";
    }

    public void FindWallToBreak()
    {
        // Gets Tuple (AR Plane: wall, Vector 3: point)
        wallPoint = findPointsOnWalls.GetRandomWallAndPoint();
    }

    public void MakeHoleInWall()
    {
        
    }
}