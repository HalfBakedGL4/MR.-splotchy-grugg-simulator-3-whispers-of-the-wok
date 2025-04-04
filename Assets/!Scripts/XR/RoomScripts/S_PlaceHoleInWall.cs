using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.ARFoundation;

public class S_PlaceHoleInWall : MonoBehaviour
{
    
    [SerializeField] private GameObject hole;

    public void SpawnHoleInWall((ARPlane wall, Vector3 pointOnWall) wallAndPoint)
    {
        
        // Instantiates and set position and rotation equal to Wall
        var instance = Instantiate(hole, wallAndPoint.wall.transform.position, wallAndPoint.wall.transform.rotation);
        // Move hole to Point on Wall
        instance.transform.SetParent(wallAndPoint.wall.transform);
        instance.transform.localPosition += wallAndPoint.pointOnWall;
    }
}
