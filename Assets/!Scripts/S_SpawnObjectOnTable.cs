using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class S_SpawnObjectOnTable : MonoBehaviour
{
    [SerializeField] private ARPlaneManager planeManager;
    [SerializeField] private PlaneClassification targetClassification;
    [SerializeField] private GameObject spawnObject;

    private Vector3 spawnObjectSize;

    private void OnEnable()
    {
        planeManager.planesChanged += PlaceObjectOnPlane;
    }

    private void OnDisable()
    {
        planeManager.planesChanged -= PlaceObjectOnPlane;
    }

    private void Start()
    {
        // Ensure we get the bounds of the object properly
        spawnObjectSize = spawnObject.GetComponentInChildren<Renderer>().bounds.size;
    }

    private void PlaceObjectOnPlane(ARPlanesChangedEventArgs obj)
    {
        List<ARPlane> newPlanes = obj.added;

        foreach (var plane in newPlanes)
        {
            if (plane.classification == targetClassification)
            {
                // Get plane center and size
                Vector2 planeSize = plane.size;

                int rows = Mathf.FloorToInt(planeSize.y / spawnObjectSize.z);
                int columns = Mathf.FloorToInt(planeSize.x / spawnObjectSize.x);

                Vector3 planeCenter = plane.transform.position;

                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < columns; j++)
                    {
                        // Calculate position relative to the plane's center
                        Vector3 localOffset = new Vector3(j * spawnObjectSize.x, 0, i * spawnObjectSize.z);
                        Vector3 spawnPosition = planeCenter + plane.transform.rotation * localOffset;

                        Instantiate(spawnObject, spawnPosition, plane.transform.rotation);
                    }
                }
            }
        }
    }
}