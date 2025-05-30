using Unity.VisualScripting;
using UnityEngine;

public class S_MenuLever : MonoBehaviour
{
    [SerializeField] GameObject lever;
    bool moveLeft;

    bool hasHappened;

    [SerializeField] Vector3 rotatePos;

    private void Start()
    {
        rotatePos = lever.transform.eulerAngles;
    }

    void OnTriggerEnter(Collider other)
    {
        Vector3 toPlayer = (other.transform.position - transform.position).normalized;
        Vector3 right = transform.forward;

        float dot = Vector3.Dot(toPlayer, right);

        if (dot > 0)
        {
            moveLeft = false;
            Debug.Log("Entered from the RIGHT");
        }
        else
        {
            moveLeft = true;
            Debug.Log("Entered from the LEFT");
        }

        if(!hasHappened)
        {
            MoveLever();
            hasHappened = true;
        }
    }

    void MoveLever()
    {
        Debug.Log(rotatePos);
        if ((rotatePos.x <= 310 && moveLeft) || (rotatePos.x >= 130 && !moveLeft))
        {
            Debug.Log("can't do that");
            return;
        }
        else
        {
            Vector3 amountToMove = new Vector3(20f, 0f, 0f);

            rotatePos += moveLeft ? amountToMove : -amountToMove;
            lever.transform.eulerAngles = rotatePos;
        }
    }

    void OnTriggerExit(Collider other)
    {
        hasHappened = false;
    }
}