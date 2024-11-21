using System;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    
    [SerializeField] private float maxXPosition;
    
    public static Player Instance { get; private set; }
    public event EventHandler<OnMoveEventArgs> OnMove;

    public class OnMoveEventArgs : EventArgs {
        
        public Vector3 Direction;
        
    }

    private const float MovementSpeed = 7f;
    private const float MinimumMovementDelay = .15f;
    
    private Vector3 _startPosition;
    private Vector3 _targetPosition;
    private float _progress;
    private Vector3 _movementQueue;
    
    private void Awake() {
        Instance = this;
        _progress = MinimumMovementDelay;
        _movementQueue = Vector3.zero;
    }

    private void Start() {
        InputManager.Instance.OnMovementButtonPressed += InputManager_OnMovementButtonPressed;
    }

    private void Update() {
        if (_progress < MinimumMovementDelay) {
            _progress += Time.deltaTime;
            transform.position = Vector3.Lerp(_startPosition, _targetPosition, _progress * MovementSpeed);
            
            if (_progress >= MinimumMovementDelay) {
                OnMove?.Invoke(this, new OnMoveEventArgs {
                    Direction = _targetPosition - _startPosition
                });
            }
        }
        else if (_movementQueue != Vector3.zero) {
            _startPosition = transform.position;
            _targetPosition = _startPosition + _movementQueue;
            _progress = 0f;

            _movementQueue = Vector3.zero;
        }
    }

    private void InputManager_OnMovementButtonPressed(object sender, InputManager.OnMovementButtonPressedArgs e) {
        Vector3 movementDirection = new Vector3(e.Direction.x, 0, e.Direction.y);
        if (_progress >= MinimumMovementDelay || _movementQueue == Vector3.zero) {
            if ((GetPositionAfterQueue() + movementDirection).x >= maxXPosition && movementDirection == Vector3.right) 
                return;
            if ((GetPositionAfterQueue() + movementDirection).x <= -maxXPosition && movementDirection == Vector3.left) 
                return;
            _movementQueue = movementDirection;
        }
    }

    private Vector3 GetPositionAfterQueue() {
        return transform.position + _movementQueue;
    }
}
