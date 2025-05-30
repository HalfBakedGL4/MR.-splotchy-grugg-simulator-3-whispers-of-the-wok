using Fusion;
using Input;
using UnityEngine;
using static Unity.Collections.Unicode;

public class S_SpawnCustomer : NetworkBehaviour
{
    [SerializeField] GameObject customer;
    [SerializeField] GameObject hole;
    Camera cam;

    // Update is called once per frame

    private void Start()
    {
        cam = Camera.main;
    }

    public void SpawnCust(InputInfo info)
    {
        if (!info.context.started) return;

        CastRay();
    }

    private void CreatePortal(Vector3 pos, Quaternion rot)
    {
        Debug.Log("Spawn Portal");
        SpawnCustomer(pos, rot);
    }

    private void CastRay()
    {
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit raycast))
        {

            Quaternion rot = raycast.transform.rotation;

            CreatePortal(raycast.point, rot);
        }
        
    }

    void SpawnCustomer(Vector3 pos, Quaternion rot)
    {
        pos.y = 0.7f;
        Runner.Spawn(customer, pos);
        

        Runner.Spawn(hole, pos, rot);
        
    }
}
