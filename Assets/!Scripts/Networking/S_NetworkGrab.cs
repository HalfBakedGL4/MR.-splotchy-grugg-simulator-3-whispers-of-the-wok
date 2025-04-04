using UnityEngine;
using Fusion;

public class S_NetworkGrab : MonoBehaviour
{

    enum Status
    {
        NotGrabbed,
        Grabbed,
        WillBeGrabbedUponAuthorityReception
    }

    Status status = Status.NotGrabbed;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
