using UnityEngine;
using Random = UnityEngine.Random;

public class Vehicle : MonoBehaviour {

    [SerializeField] private VehicleSO vehicleSo;

    private int _speed;

    public void Start() {
        
        float playerXPosition = Player.Instance.transform.position.x;
        if (transform.position.x < playerXPosition) {
            transform.forward = Vector3.right;
        }
        else {
            transform.forward = Vector3.left;
        }
        
    }

    public void Update() {
        // todo: better checking position of a vehicle
        
        transform.position += transform.forward * (_speed * Time.deltaTime);
    }

    public void SetSpeed(int speed) {
        _speed = speed;
    }
}
