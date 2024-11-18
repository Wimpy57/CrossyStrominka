using System;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    
    public static Player Instance { get; private set; }
    public event EventHandler OnMoveForward;
    public event EventHandler OnMoveBackward;

    private const float MovementSpeed = 7f;
    private const float MinimumMovementDelay = .15f;
    private const int MaxMovementQueueLenght = 3;
    
    private Vector3 _startPosition;
    private Vector3 _targetPosition;
    private float _progress;
    private List<Vector3> _movementQueue;
    
    private void Awake() {
        Instance = this;
        _progress = MinimumMovementDelay;
        _movementQueue = new List<Vector3>();
    }

    private void Start() {
        InputManager.Instance.OnMovementButtonPressed += InputManager_OnMovementButtonPressed;
    }

    private void Update() {
        if (_progress < MinimumMovementDelay) {
            _progress += Time.deltaTime;
            transform.position = Vector3.Lerp(_startPosition, _targetPosition, _progress * MovementSpeed);
            if (_progress >= MinimumMovementDelay) {
                if (_targetPosition.z > _startPosition.z) {
                    OnMoveForward?.Invoke(this, EventArgs.Empty);
                }
                else {
                    OnMoveBackward?.Invoke(this, EventArgs.Empty);
                }
            }
        }
        else if (_movementQueue.Count > 0) {
            _startPosition = transform.position;
            _targetPosition = _startPosition + _movementQueue[0];
            _progress = 0f;
            
            _movementQueue.RemoveAt(0);
        }
    }

    private void InputManager_OnMovementButtonPressed(object sender, InputManager.OnMovementButtonPressedArgs e) {
            Vector3 movementDirection = new Vector3(e.Direction.x, 0, e.Direction.y);
            if (_progress >= MinimumMovementDelay || _movementQueue.Count < MaxMovementQueueLenght) {
                _movementQueue.Add(movementDirection);
            }
    }
}
