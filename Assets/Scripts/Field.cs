using UnityEngine;

public class Field : MonoBehaviour {

    [SerializeField] private Spawner[] spawners;
    [SerializeField] private GameObject fieldVisual;
    
    private Renderer _visualRenderer;

    private void Awake() {
        _visualRenderer = fieldVisual.GetComponent<Renderer>();
    }

    private void Start() {
        if (spawners.Length != 0) {
            ChooseSpawner();
        }
    }
    
    public Vector3 GetExtents() {
        return _visualRenderer.bounds.extents;
    }

    private void ChooseSpawner() {
        Spawner spawner = spawners[Random.Range(0, spawners.Length)];
        spawner.SpawnVehicle();
    }
}
