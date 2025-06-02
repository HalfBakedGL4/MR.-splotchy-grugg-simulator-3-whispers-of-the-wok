using Fusion;
using System;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using Vector3 = UnityEngine.Vector3;

[Serializable]
public class Station
{
    public GameObject toSpawn;

    public int amount;

    public float maxHeight; //zero will do nothing
    public Vector3 offset;
    public Vector3 eulerOffset;

    [ReadOnly] public bool spawnedAllInstances;


    public Station(GameObject toSpawn, Vector3 offset = default, Vector3 eulerOffset = default, int amount = 1, bool spawnedAllInstances = false)
    {
        this.toSpawn = toSpawn;
        this.amount = amount;
        this.spawnedAllInstances = spawnedAllInstances;

        this.offset = offset;
        this.eulerOffset = eulerOffset;

        //name = toSpawn.name;
    }
}
[Serializable]
public class SpawnableStation
{
    public string name;

    public Station[] stations;

    public PlaneClassifications toPlaceOn;

    public SpawnableStation(Station[] stations, PlaneClassifications toPlaceOn = PlaneClassifications.Table)
    {
        this.stations = stations;
        this.toPlaceOn = toPlaceOn;

        name = this.stations[0].toSpawn.name;
    }
}
public class S_SpawnObjectOnClassification : NetworkBehaviour
{
    [SerializeField, ReadOnly] private ARPlaneManager planeManager;

    [SerializeField] SpawnableStation[] spawnableStations;

    private bool islocal => Object && Object.HasStateAuthority;

    private void OnValidate()
    {
        if (planeManager == null)
            planeManager = FindFirstObjectByType<ARPlaneManager>();
    }

    public override void Spawned()
    {
        if (!islocal) return;

        if (planeManager == null)
            planeManager = FindFirstObjectByType<ARPlaneManager>();
        Debug.Log("[Spawn Objects] Network object spawned in Shared Mode.");
        PlaceObjectOnPlane(planeManager.trackables);
    }

    // Needs to be referenced in the editor in ARPlaneManager
    public async void PlaceObjectOnPlane(TrackableCollection<ARPlane> changes)
    {
        foreach (var spawnableStation in spawnableStations)
        {
            foreach (var surface in changes)
            {
                foreach (var station in spawnableStation.stations)
                {

                    if (station.spawnedAllInstances == true) //checking if we should spawn more of the station
                        continue;

                    if (spawnableStation.toPlaceOn.HasFlag(surface.classifications)) //checking if the classifications match
                    {
                        Debug.Log("[Spawn Objects] try spawn " + station.toSpawn + " on " + surface.classifications);

                        //getting the appropriate position and rotation
                        Vector3 position = surface.transform.position + station.offset;
                        Quaternion rotation = Quaternion.Euler(surface.transform.eulerAngles + station.eulerOffset);

                        //checking height
                        if (station.maxHeight != 0) 
                        {
                            if(position.y > station.maxHeight)
                            {
                                position.y = station.maxHeight;
                            }
                        }

                        //spawning the object
                        NetworkObject obj = Runner.Spawn(station.toSpawn, position, rotation);

                        if(obj != null)
                        {
                            Debug.Log("[Spawn Objects] successfully spawned " + station.toSpawn + " on " + surface.classifications);
                        } else
                        {
                            Debug.Log("[Spawn Objects] failed to spawn " + station.toSpawn + " on " + surface.classifications);
                        }

                            station.amount--;

                        if(station.amount <= 0)
                            station.spawnedAllInstances = true;

                        await Task.Delay(1000);

                        break;
                    }
                }

            }
        }
    }

}
