using UnityEngine;
using UnityEngine.Serialization;

public class Spawner : MonoBehaviour {

    [SerializeField] private VehicleSO vehicleSo;
    [SerializeField] private Transform field;
    
    public void SpawnVehicle() {
        Transform vehicle = Instantiate(vehicleSo.prefab, transform.position, Quaternion.identity, field);
        
    }
}
