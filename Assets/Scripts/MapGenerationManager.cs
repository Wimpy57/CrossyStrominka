using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MapGenerationManager : MonoBehaviour {

    [SerializeField] private RoadLineSO[] roadLineSoList;
    [SerializeField] private Field lastField;
    [SerializeField] private Transform mapContainer;

    private const int MaxSameFieldsSpawnedInRow = 4;
    private const int MaxSpawnedFieldsInDirection = 15;
    private const int MaxStepsBackCount = 5;
    private const float StepsBackCounterResetAfterTime = 5f;

    private List<Field> _spawnedFields;
    private int _lastSpawnedFieldIndex;
    private int _stepsBackCount;
    private float _stepsBackResetTimer;
    private List<int> _fieldIndexesToSpawn;

    private void Start() {
        
        _spawnedFields = new List<Field> {lastField};
        _fieldIndexesToSpawn = new List<int>();
        GenerateMapOnStart();
        
        Player.Instance.OnMoveForward += Player_OnMoveForward;
        Player.Instance.OnMoveBackward += Player_OnMoveBackward;
    }


    private void Player_OnMoveForward(object sender, EventArgs e) {
        if (_fieldIndexesToSpawn.Count == 0) {
            int fieldToSpawnIndex = GetRandomFieldToSpawnIndex();
            int spawnedFieldsAmount = Random.Range(1, MaxSameFieldsSpawnedInRow + 1);

            for (int i = 0; i < spawnedFieldsAmount; i++) {
                _fieldIndexesToSpawn.Add(fieldToSpawnIndex);
            }
        }
        
        GenerateFields(_fieldIndexesToSpawn[0]);
        RemoveLastField();
        _fieldIndexesToSpawn.RemoveAt(0);
    }
    
    private void Player_OnMoveBackward(object sender, EventArgs e) {
        _stepsBackCount++;
        if (_stepsBackCount >= MaxStepsBackCount) {
            //todo: it can be reset while player is making fast steps back, so he can do much more than 5
            Debug.LogWarning("Max steps back count reached");
        }
    }

    private void Update() {
        _stepsBackResetTimer += Time.deltaTime;

        if (_stepsBackResetTimer >= StepsBackCounterResetAfterTime) {
            _stepsBackResetTimer = 0f;
        }
    }

    private void GenerateFields(int fieldToSpawnIndex, int count=1, bool generateInFront=true) {
        RoadLineSO roadLineSo = roadLineSoList[fieldToSpawnIndex];
        int multiplier = generateInFront ? 1 : -1;
        
        for (int i = 0; i < count; i++) {
            
            Vector3 lastFieldExtents = lastField.GetExtents();
            Field newField = Instantiate(roadLineSo.prefab, 
                Vector3.zero, Quaternion.identity, mapContainer).GetComponent<Field>();
            Vector3 newFieldExtents = newField.GetExtents();
            
            newField.transform.position = new Vector3(0f, 0f, 
                lastField.transform.position.z + (newFieldExtents.z + lastFieldExtents.z) * multiplier);

            if (generateInFront) {
                _spawnedFields.Add(newField);
            }
            else {
                _spawnedFields.Insert(0, newField);
            }
            lastField = newField;
        }
        _lastSpawnedFieldIndex = fieldToSpawnIndex;
    }


    private void RemoveLastField() {
        Destroy(_spawnedFields[0].gameObject);
        _spawnedFields.RemoveAt(0);
    }
    
    private int GetRandomFieldToSpawnIndex() {
        while (true) { 
            int fieldToSpawnIndex = Random.Range(0, roadLineSoList.Length);
            if (fieldToSpawnIndex != _lastSpawnedFieldIndex) {
                return fieldToSpawnIndex;
            }
        }
    }

    private int GetSidewalkFieldIndex() {
        for (int i = 0; i < roadLineSoList.Length; i++) {
            if (roadLineSoList[i].roadName == "Sidewalk field") {
                return i;
            }
        }

        return -1;
    }

    private void GenerateMapOnStart() {
        int startFieldIndex = GetSidewalkFieldIndex();
        
        if (startFieldIndex == -1) {
            Debug.LogError("There is no sidewalk field in the list.");
        }
        
        int maxFieldsInFront = 7;
        int minFieldsInFront = 3;
        GenerateFields(startFieldIndex, MaxSpawnedFieldsInDirection, generateInFront:false);
        lastField = _spawnedFields[MaxSpawnedFieldsInDirection];
        GenerateFields(startFieldIndex, Random.Range(minFieldsInFront, maxFieldsInFront), generateInFront:true);
    }
}
