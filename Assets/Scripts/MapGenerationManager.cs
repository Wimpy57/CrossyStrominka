using System.Collections.Generic;
using UnityEngine;

public class MapGenerationManager : MonoBehaviour {

    [SerializeField] private RoadLineSO[] roadLineSoList;
    [SerializeField] private Field lastField;

    private const float GenerationTimerMax = 2f;
    private const int MaxFieldsSpawnedInRow = 4;
    private const int MaxSpawnedFieldsInDirection = 15;

    private List<Field> _spawnedFields;
    private float _generationTimer;
    private int _lastSpawnedFieldIndex;

    private void Start() {
        _spawnedFields = new List<Field>();
        GenerateMapOnStart();
    }
    
    private void Update() {
        if (_generationTimer >= GenerationTimerMax) {
            _generationTimer = 0f;

            int fieldToSpawnIndex = GetFieldToSpawnIndex();
            
            GenerateFields(fieldToSpawnIndex, Random.Range(1, MaxFieldsSpawnedInRow+1));
        }
        
        _generationTimer += Time.deltaTime;
    }

    private void GenerateFields(int fieldToSpawnIndex, int count, bool generateInFront=true) {
        RoadLineSO roadLineSo = roadLineSoList[fieldToSpawnIndex];
        int multiplier = generateInFront ? 1 : -1;
        
        for (int i = 0; i < count; i++) {
            
            Vector3 lastFieldExtents = lastField.GetExtents();
            Field newField = Instantiate(roadLineSo.prefab, 
                Vector3.zero, Quaternion.identity).GetComponent<Field>();
            Vector3 newFieldExtents = newField.GetExtents();
            
            newField.transform.position = new Vector3(0f, 0f, 
                lastField.transform.position.z + (newFieldExtents.z + lastFieldExtents.z) * multiplier);

            if (generateInFront) {
                _spawnedFields.Add(newField);
            }
            lastField = newField;
        }
        _lastSpawnedFieldIndex = fieldToSpawnIndex;
    }

    private int GetFieldToSpawnIndex() {
        while (true) { 
            int fieldToSpawnIndex = Random.Range(0, roadLineSoList.Length);
            if (fieldToSpawnIndex != _lastSpawnedFieldIndex) {
                return fieldToSpawnIndex;
            }
        }
    }

    private void GenerateMapOnStart() {
        Field startField = lastField;
        GenerateFields(2, MaxSpawnedFieldsInDirection, generateInFront:false);
        lastField = startField;
        GenerateFields(2, MaxSpawnedFieldsInDirection);
    }
}
