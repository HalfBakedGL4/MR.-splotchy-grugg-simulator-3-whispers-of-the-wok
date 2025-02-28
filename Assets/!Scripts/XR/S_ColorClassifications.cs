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
    
    // Needs to be referenced in the editor in ARPlaneManager
    public void SetupPlane(ARTrackablesChangedEventArgs<ARPlane> changes)
    {
        foreach (var item in changes.added)
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

