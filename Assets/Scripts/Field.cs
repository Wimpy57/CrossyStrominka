using UnityEngine;

public class Field : MonoBehaviour {

    [SerializeField] private Spawner[] spawners;
    
    private Renderer _renderer;

    private void Awake() {
        _renderer = gameObject.GetComponent<Renderer>();
    }

    private void Start() {
        if (spawners.Length != 0) {
            ChooseSpawner();
        }
    }
    
    public Vector3 GetExtents() {
        return _renderer.bounds.extents;
    }

    private void ChooseSpawner() {
        Spawner spawner = spawners[Random.Range(0, spawners.Length)];
        spawner.SpawnVehicle();
    }
}
