using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

public class S_SpawnObjectOnClassification : MonoBehaviour
{
    [Header("Link this script to AR Plane Manager!!")]
    [SerializeField] private PlaneClassifications classifications;
    [SerializeField] private GameObject[] spawnObjects;
    
    [SerializeField] private S_OrderWindow orderWindow;
    [SerializeField] private float windowHeight = 1.0f;
    private float basketSize = 0.5f;
    
    private bool windowPlaced = false;

    private ARPlaneManager planeManager;

    private void OnEnable()
    {
        planeManager = FindFirstObjectByType<ARPlaneManager>();
        planeManager.trackablesChanged.AddListener(PlaceObjectOnPlane);
    }

    // Needs to be referenced in the editor in ARPlaneManager
    void PlaceObjectOnPlane(ARTrackablesChangedEventArgs<ARPlane> changes)
    {
        foreach (var item in changes.added)
        {
            // Place applications on table across the room
            if (item.classifications == classifications)
            {
                // Finds number of rows and columns to create a grid to place items
                var rows = item.size.x / basketSize;
                var cols = item.size.y / basketSize;

                rows = Mathf.FloorToInt(rows);
                cols = Mathf.FloorToInt(cols);

                var rowSize = item.size.x / rows;
                var colSize = item.size.y / cols;

                //debugText.text += item.name + ": " + item.size + " Rows: " + rows + " Cols: " + cols + "\n";

                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < cols; j++)
                    {
                        // Offset to place items in middle of grid square
                        var offsetVector = new Vector3(rowSize * (i + 0.5f), 0, colSize * (j + 0.5f)) -
                                           new Vector3(item.extents.x, 0, item.extents.y);
                        // Spawn Object
                        var objectInstance = Instantiate(spawnObjects[Random.Range(0,spawnObjects.Length)], item.transform.position, Quaternion.identity);
                        // Make Object Child of ARPlane to place it through localTransform
                        objectInstance.transform.parent = item.transform;
                        objectInstance.transform.localPosition += offsetVector;
                        // Where to spawn object in world Space
                        //debugText.text += objectInstance.name + ": " + objectInstance.transform.localPosition + "\n";
                    }
                }
            }

            if (item.classifications == PlaneClassifications.WallFace && !windowPlaced)
            {
                var windowInstance = Instantiate(orderWindow, item.transform.position, Quaternion.identity);
                
                windowInstance.transform.parent = item.transform;
                windowInstance.transform.localEulerAngles = new Vector3(180, -90, -90);
                windowInstance.transform.position = new Vector3(windowInstance.transform.position.x, windowHeight, windowInstance.transform.position.z);
                
                windowPlaced = true;
            }
        }
    }

    private void OnDisable()
    {
        planeManager.trackablesChanged.RemoveListener(PlaceObjectOnPlane);
    }
}
