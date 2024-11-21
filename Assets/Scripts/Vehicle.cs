using UnityEngine;

public class Vehicle : MonoBehaviour {

    [SerializeField] private VehicleSO vehicleSo;

    private int _speed;
    private float _startXPosition;

    public void Start() {
        
        float playerXPosition = Player.Instance.transform.position.x;
        if (transform.position.x < playerXPosition) {
            transform.forward = Vector3.right;
        }
        else {
            transform.forward = Vector3.left;
        }
        
        _startXPosition = transform.position.x;
    }

    public void Update() {
        transform.position += transform.forward * (_speed * Time.deltaTime);

        if (Mathf.Abs(transform.position.x) > Mathf.Abs(_startXPosition)) {
            Destroy(gameObject);
        }

    }

    public void SetSpeed(int speed) {
        _speed = speed;
    }
}
