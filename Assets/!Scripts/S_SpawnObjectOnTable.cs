using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class S_SpawnObjectOnTable : MonoBehaviour
{
    [SerializeField] private ARPlaneManager planeManager;
    [SerializeField] private PlaneClassifications classifications;
    
    [SerializeField] private GameObject spawnObject;
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
                Instantiate(spawnObject, item.transform.position, Quaternion.identity);
            }
        }
    }
}
