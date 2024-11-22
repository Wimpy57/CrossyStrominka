using System;
using UnityEngine;

public class Player : MonoBehaviour {
    
    [SerializeField] private float maxXPosition;
    
    public static Player Instance { get; private set; }

    public event EventHandler OnCollideVehicle;
    public event EventHandler<OnMoveEventArgs> OnMove;

    public class OnMoveEventArgs : EventArgs {
        
        public Vector3 Direction;
        
    }

    private const string PeopleLayerMask = "People";
    private const string VehicleLayerMask = "Vehicle";
    private const float MovementSpeed = 7f;
    private const float MinimumMovementDelay = .15f;
    
    private Vector3 _startPosition;
    private Vector3 _targetPosition;
    private Vector3 _movementQueue;
    private float _progress;
    
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

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.layer == LayerMask.NameToLayer(VehicleLayerMask)) {
            DisableMovement();
            OnCollideVehicle?.Invoke(this, EventArgs.Empty);
        }
    }

    private void InputManager_OnMovementButtonPressed(object sender, InputManager.OnMovementButtonPressedArgs e) {
        Vector3 movementDirection = new Vector3(e.Direction.x, 0, e.Direction.y);
        if (_progress >= MinimumMovementDelay || _movementQueue == Vector3.zero) {
            if ((GetPositionAfterQueue() + movementDirection).x >= maxXPosition && movementDirection == Vector3.right) 
                return;
            if ((GetPositionAfterQueue() + movementDirection).x <= -maxXPosition && movementDirection == Vector3.left) 
                return;
            
            bool canMove = !Physics.Raycast(transform.position,
                movementDirection, 1f, LayerMask.GetMask(PeopleLayerMask));
            if (canMove) {
                _movementQueue = movementDirection;
            }
        }
    }

    private Vector3 GetPositionAfterQueue() {
        return transform.position + _movementQueue;
    }

    public void DisableMovement() {
        InputManager.Instance.OnMovementButtonPressed -= InputManager_OnMovementButtonPressed;
    }
}
