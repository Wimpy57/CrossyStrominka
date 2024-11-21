using System;
using Mono.Cecil;
using UnityEngine;
using Random = UnityEngine.Random;


public class Spawner : MonoBehaviour {

    [SerializeField] private VehicleSO vehicleSo;

    private const float MaxSpawnDelay = 5f;
    private const float MinSpawnDelay = 1f;

    private float _spawnDelay;
    private int _vehicleToSpawnSpeed;
    private Field _parentField;

    private void Awake() {
        _vehicleToSpawnSpeed = Random.Range(vehicleSo.minSpeed, vehicleSo.maxSpeed);
    }

    private void Update() {
        if (_spawnDelay <= 0f) {
            SpawnVehicle();
            _spawnDelay = Random.Range(MinSpawnDelay, MaxSpawnDelay);
        }
        else {
            _spawnDelay -= Time.deltaTime;
        }
    }
    
    private void SpawnVehicle() {
        if (_parentField == null) return;
        Transform vehicle = Instantiate(vehicleSo.prefab, transform.position, Quaternion.identity, _parentField.transform);
        vehicle.GetComponent<Vehicle>().SetSpeed(_vehicleToSpawnSpeed);
    }

    public void SetParentField(Field field) {
        _parentField = field;
    }
}
