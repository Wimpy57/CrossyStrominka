using UnityEngine;

public class Field : MonoBehaviour {

    private Renderer _renderer;

    private void Awake() {
        _renderer = gameObject.GetComponent<Renderer>();
    }

    public Vector3 GetExtents() {
        return _renderer.bounds.extents;
    }
}
