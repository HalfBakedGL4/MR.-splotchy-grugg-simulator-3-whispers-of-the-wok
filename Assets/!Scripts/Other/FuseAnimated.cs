using UnityEngine;

public class FuseAnimated : MonoBehaviour
{
    public float burnSpeed = 0.05f;
    void Update()
    {
        transform.localPosition -= transform.forward * (burnSpeed * Time.deltaTime);
    }
}
