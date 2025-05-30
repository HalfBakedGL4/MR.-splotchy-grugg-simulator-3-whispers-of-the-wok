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

            Quaternion rot = Quaternion.Euler(0, raycast.transform.rotation.y, 0);

            CreatePortal(raycast.point, rot);
        }
        
    }

    void SpawnCustomer(Vector3 pos, Quaternion rot)
    {
        rot.z = 90;
        Runner.Spawn(hole, pos, rot);
        Runner.Spawn(customer, pos, rot);
    }
}
