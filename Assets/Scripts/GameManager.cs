using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour {

    [SerializeField] private RoadLineSO[] roadLineSoList;
    [SerializeField] private Field lastField;
    [SerializeField] private Transform mapContainer;
    [SerializeField] private Transform humanPrefab;
    
    private const int MaxSameFieldsSpawnedInRow = 4;
    private const int MaxSpawnedFieldsInDirection = 15;
    private const int MaxStepsBackCount = 5;
    private const float StepsBackCounterResetAfterTime = 5f;
    private const int MaxPeopleOnField = 10;

    private List<Field> _spawnedFields;
    private int _lastSpawnedFieldIndex;
    private int _stepsBackCount;
    private float _stepsBackResetTimer;
    private List<int> _fieldIndexesToSpawn;

    private void Start() {
        
        _spawnedFields = new List<Field> {lastField};
        _fieldIndexesToSpawn = new List<int>();
        GenerateMapOnStart();
        
        Player.Instance.OnMove += Player_OnMove;
    }

    private void Player_OnMove(object sender, Player.OnMoveEventArgs e) {
        if (e.Direction == Vector3.forward) {
            OnPlayerMoveForward();
        } 
        else if (e.Direction == Vector3.back) {
            OnPlayerMoveBackward();    
        }
    }


    private void OnPlayerMoveForward() {
        if (_fieldIndexesToSpawn.Count == 0) {
            int fieldToSpawnIndex = GetRandomFieldToSpawnIndex();
            int spawnedFieldsAmount = Random.Range(1, MaxSameFieldsSpawnedInRow + 1);

            for (int i = 0; i < spawnedFieldsAmount; i++) {
                _fieldIndexesToSpawn.Add(fieldToSpawnIndex);
            }
        }
        
        GenerateFields(_fieldIndexesToSpawn[0]);
        _fieldIndexesToSpawn.RemoveAt(0);
        RemoveLastField();
    }
    
    private void OnPlayerMoveBackward() {
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

            if (!(newField is FieldWithSpawner)) {
                SpawnCrowdsOnField(newField.transform);
            }
            
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
        int startFieldsInFrontAmount = Random.Range(minFieldsInFront, maxFieldsInFront);
        GenerateFields(startFieldIndex, startFieldsInFrontAmount, generateInFront:true);
        
        for (int i = 0; i < MaxSpawnedFieldsInDirection - startFieldsInFrontAmount; i++) {
            if (_fieldIndexesToSpawn.Count == 0) {
                int fieldToSpawnIndex = GetRandomFieldToSpawnIndex();
                int spawnedFieldsAmount = Random.Range(1, MaxSameFieldsSpawnedInRow + 1);

                for (int j = 0; j < spawnedFieldsAmount; j++) {
                    _fieldIndexesToSpawn.Add(fieldToSpawnIndex);
                }
            }
            
            GenerateFields(_fieldIndexesToSpawn[0]);
            _fieldIndexesToSpawn.RemoveAt(0);
        }
    }

    private void SpawnCrowdsOnField(Transform fieldTransform) {
        //todo: bigger spawning chance in the middle of the map
        int spawnedPeople = 0;
        for (int i = 10; i >= 0; i--) {
            if (Random.Range(0, 10) <= i && spawnedPeople <= MaxPeopleOnField) {
                InstantiateHuman(fieldTransform, i);
                spawnedPeople++;
            }
            if (i > 0 && Random.Range(0, 10) <= i && spawnedPeople <= MaxPeopleOnField) {
                InstantiateHuman(fieldTransform, -i);
                spawnedPeople++;
            }
        }
    }

    private void InstantiateHuman(Transform fieldTransform, int xPosition) {
        Instantiate(humanPrefab, fieldTransform);
        humanPrefab.SetPositionAndRotation(new Vector3(xPosition, 0f, 0f), Quaternion.identity);
    }
}
