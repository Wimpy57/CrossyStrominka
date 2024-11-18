using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Vehicle : MonoBehaviour {

    [SerializeField] private VehicleSO vehicleSo;

    private int _speed;

    public void Start() {
        _speed = Random.Range(vehicleSo.minSpeed, vehicleSo.maxSpeed);
        
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
        if (transform.position.x != transform.position.x * (-1)) {
            transform.position += Vector3.right * (_speed * Time.deltaTime);
        }
    }
    
    
}
