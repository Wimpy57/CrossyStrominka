using UnityEngine;

public class MapGenerationManager : MonoBehaviour {

    [SerializeField] private RoadLineSO[] roadLineSoList;    
    [SerializeField] private Field lastField;


    private const float GenerationTimerMax = 2f;
    
    private float _generationTimer;

    
    private void Update() {
        if (_generationTimer >= GenerationTimerMax) {
            _generationTimer = 0f;
            
            Vector3 lastFieldExtents = lastField.GetExtents();
            
            Field newField = Instantiate(roadLineSoList[Random.Range(0, roadLineSoList.Length)].prefab, 
                Vector3.zero, Quaternion.identity).GetComponent<Field>();
            Vector3 newFieldExtents = newField.GetExtents();
            
            newField.transform.position = new Vector3(0f, 0f, 
                lastField.transform.position.z + newFieldExtents.z + lastFieldExtents.z);
            
            lastField = newField;
        }
        
        _generationTimer += Time.deltaTime;
    }
}
