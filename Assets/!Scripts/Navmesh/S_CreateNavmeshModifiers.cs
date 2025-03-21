using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class S_CreateNavmeshModifiers : MonoBehaviour
{
    [SerializeField] private PlaneClassifications obstacle_classifications;
    [SerializeField] private PlaneClassifications walkable_classifications;
    [SerializeField] private GameObject obstacleCube;
    [SerializeField] private GameObject walkableCube;

    public void CreateNavmeshModifiers(ARTrackablesChangedEventArgs<ARPlane> changes)
    {
        foreach (var item in changes.added)
        {
            if (item.classifications == obstacle_classifications)
            {
                var objectInstance = Instantiate(obstacleCube, item.transform.position, Quaternion.identity);
                objectInstance.transform.parent = item.transform;
                Vector3 objectScale = new Vector3(item.size.x, item.size.y, 100);
                objectInstance.transform.localScale = objectScale;
            } else if (item.classifications == walkable_classifications)
            {
                var objectInstance = Instantiate(walkableCube, item.transform.position, Quaternion.identity);
                objectInstance.transform.parent = item.transform;
                Vector3 objectScale = new Vector3(item.size.x, item.size.y, 1);
                objectInstance.transform.localScale = objectScale;
            }
        }
    }
}
