using UnityEngine;
using UnityEngine.Serialization;

public class Spawner : MonoBehaviour {

    [SerializeField] private VehicleSO vehicleSo;

    public void SpawnVehicle() {
        Instantiate(vehicleSo.prefab, transform.position, Quaternion.identity);
    }
}
