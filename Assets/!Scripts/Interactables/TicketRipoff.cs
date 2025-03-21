using ExitGames.Client.Photon.StructWrapping;
using UnityEngine;

public class TicketRipoff : MonoBehaviour
{
    [SerializeField] private GameObject ticketBottomSide;
    private Rigidbody rbTicketBottomSide;

    private void Start() 
    {
        rbTicketBottomSide = ticketBottomSide.GetComponent<Rigidbody>();
    }

    private void FixedUpdate() 
    {
        if (!GetComponent<FixedJoint>()) 
        {
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            //Destroy(this);

            rbTicketBottomSide.useGravity = true;
        }
    }
}
