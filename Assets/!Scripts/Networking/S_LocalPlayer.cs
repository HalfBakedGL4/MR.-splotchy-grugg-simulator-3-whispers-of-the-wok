using UnityEngine;
using Fusion;

public class S_LocalPlayer : MonoBehaviour
{
    public S_HardwarePart head, rightHand, leftHand;

    [SerializeField] NetworkRunner runner;

    private void Start()
    {
        transform.position = new Vector3(Random.Range(-5, 5), 0, Random.Range(-5, 5));
    }

    private void Update()
    {
        name = runner.LocalPlayer.ToString();
    }
}
