using UnityEngine;

public class HoleOnHit : MonoBehaviour
{
    bool spawnedWall = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            spawnedWall = true;
            SpawnWall(collision.GetContact(0).point, collision.gameObject.transform.localRotation);
        }
        
    }

    void SpawnWall(Vector3 pos, Quaternion rot)
    {
        S_HoleSpawner holeSpawner = FindAnyObjectByType<S_HoleSpawner>();
        holeSpawner.SpawnHole(pos, rot);

    }
}
