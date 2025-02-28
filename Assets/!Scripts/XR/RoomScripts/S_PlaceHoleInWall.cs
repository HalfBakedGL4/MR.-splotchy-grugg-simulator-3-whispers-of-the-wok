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
            var wallAndPoint = _findPointsOnWalls.GetRandomWallAndPoint();
            var instance = Instantiate(hole, wallAndPoint.Item1.transform.position, wallAndPoint.Item1.transform.rotation);
            instance.transform.SetParent(wallAndPoint.Item1.transform);
            instance.transform.localPosition += wallAndPoint.Item2;
        }
    }
}
