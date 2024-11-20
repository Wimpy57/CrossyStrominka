using UnityEngine;

public class Field : MonoBehaviour {

    [SerializeField] private GameObject fieldVisual;
    
    private Renderer _visualRenderer;

    private void Awake() {
        _visualRenderer = fieldVisual.GetComponent<Renderer>();
    }
    
    public Vector3 GetExtents() {
        return _visualRenderer.bounds.extents;
    }

}
