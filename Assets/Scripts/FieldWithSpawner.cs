using UnityEngine;

public class FieldWithSpawner : Field {
    
    [SerializeField] private Transform spawnerPrefab;
    [SerializeField] private Vector3[] spawnerPositions;
    
    private void Start() {
        CreateSpawner();
    }
    
    private void CreateSpawner() {
        Vector3 spawnerPosition = spawnerPositions[Random.Range(0, spawnerPositions.Length)];
        Transform spawnerTransform = Instantiate(spawnerPrefab, transform);
        spawnerTransform.SetLocalPositionAndRotation(spawnerPosition, Quaternion.identity);
        spawnerTransform.GetComponent<Spawner>().SetParentField(this);
    }
}
