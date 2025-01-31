using UnityEngine;
using Fusion;

public class Test : NetworkBehaviour, IAfterSpawned
{
    [Networked]
    public float hp { get; set; }

    public override void FixedUpdateNetwork()
    {
        if (!Runner.IsPlayer) return;
        base.FixedUpdateNetwork();
        hp -= Time.fixedDeltaTime;
    }

    public void AfterSpawned()
    {
        if (!Runner.IsPlayer) return;
        print(Runner.LocalPlayer + " " + name);
        hp = 100;
    }


}


/*
 * using UnityEngine;

public class GridPlacer : MonoBehaviour
{

    void PlaceBaskets()
    {
        // Get plane size
        Vector3 planeSize = plane.transform.localScale * 10; // Plane default size is 10x10 units

        // Basket size (assuming a BoxCollider)
        Vector3 basketSize = basketPrefab.GetComponent<Renderer>().bounds.size;

        // Calculate grid dimensions
        int rows = Mathf.FloorToInt(planeSize.z / basketSize.z);
        int columns = Mathf.FloorToInt(planeSize.x / basketSize.x);

        // Starting point offset (centered on the plane)
        Vector3 startPos = plane.transform.position 
                            - new Vector3(planeSize.x / 2, 0, planeSize.z / 2) 
                            + new Vector3(basketSize.x / 2, 0, basketSize.z / 2);

        // Loop through rows and columns
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                // Calculate position for each basket
                Vector3 position = startPos + new Vector3(j * basketSize.x, 0, i * basketSize.z);

                // Instantiate the basket at the position
                Instantiate(basketPrefab, position, Quaternion.identity);
            }
        }
    }
}

 */