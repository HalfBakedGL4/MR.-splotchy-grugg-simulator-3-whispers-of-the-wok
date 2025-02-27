using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class S_ColorClassifications : MonoBehaviour
{
    [SerializeField] private ARPlaneManager planeManager;
    [SerializeField] private PlaneClassifications showClassifications;
    [SerializeField] private PlaneClassifications portalClassifications;
    [SerializeField] private Material portalMaterial;

    
    private void OnEnable()
    {
        planeManager.planesChanged += SetupPlane;
    }
    private void OnDisable()
    {
        planeManager.planesChanged -= SetupPlane;
    }

    private void SetupPlane(ARPlanesChangedEventArgs obj)
    {
        List<ARPlane> newPlane = obj.added;
        foreach (var item in newPlane)
        {
            if (item.classifications == showClassifications)
            {
                
            }
            else if (item.classifications == portalClassifications)
            {
                item.GetComponent<Renderer>().material = portalMaterial;
            }
            else
            {
                Renderer itemRenderer = item.GetComponent<Renderer>();
                Destroy(itemRenderer);
            }
            
        }
    }

    
}

