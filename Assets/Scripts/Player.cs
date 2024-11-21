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
                OnMove?.Invoke(this, new OnMoveEventArgs {
                    Direction = _targetPosition - _startPosition
                });
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
            if ((GetPositionAfterQueue() + movementDirection).x >= maxXPosition && movementDirection == Vector3.right) 
                return;
            if ((GetPositionAfterQueue() + movementDirection).x <= -maxXPosition && movementDirection == Vector3.left) 
                return;
            _movementQueue.Add(movementDirection);
        }
    }

    private Vector3 GetPositionAfterQueue() {
        Vector3 positionAfterQueue = transform.position;
        foreach (Vector3 movement in _movementQueue) {
            positionAfterQueue += movement;
        }
        
        return positionAfterQueue;
    }
}
