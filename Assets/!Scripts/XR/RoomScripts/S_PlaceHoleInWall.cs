using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class S_PlaceHoleInWall : MonoBehaviour
{
    
    [SerializeField ]private S_FindPointsOnWalls _findPointsOnWalls;
    [SerializeField] private GameObject hole;
    [SerializeField] private InputActionProperty inputAction;
    
    private void Start()
    {
        if (!_findPointsOnWalls)
            Debug.LogError("No S_FindPointsOnWalls component found");
    }

    void Update()
    {
        if (inputAction.action.WasPressedThisFrame())
        {
            SpawnHoleInWall();
        }
    }

    private void SpawnHoleInWall()
    {
        // Gets Tuple (AR Plane: wall, Vector 3: point)
        var wallAndPoint = _findPointsOnWalls.GetRandomWallAndPoint();
        // Instantiates and set position and rotation equal to Wall
        var instance = Instantiate(hole, wallAndPoint.Item1.transform.position, wallAndPoint.Item1.transform.rotation);
        // Move hole to Point on Wall
        instance.transform.SetParent(wallAndPoint.Item1.transform);
        instance.transform.localPosition += wallAndPoint.Item2;
    }
}
