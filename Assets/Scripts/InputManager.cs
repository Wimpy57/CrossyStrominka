using System;
using UnityEngine;

public class InputManager : MonoBehaviour {

    
    public static InputManager Instance { get; private set; }
    public event EventHandler<OnMovementButtonPressedArgs> OnMovementButtonPressed;
    
    public class OnMovementButtonPressedArgs : EventArgs {
        
        public Vector2 Direction;
    }
    
    private InputController _inputController;

    private void Awake() {
        Instance = this;
    }
    
    private void Start() {
        _inputController = new InputController();
        _inputController.Enable();
    }

    private void Update() {
        // handle the movement
        if (Input.GetKeyDown(KeyCode.W)) {
            OnMovementButtonPressed?.Invoke(this, new OnMovementButtonPressedArgs {
                Direction = Vector2.up
            });
        }
        
        else if (Input.GetKeyDown(KeyCode.S)) {
            OnMovementButtonPressed?.Invoke(this, new OnMovementButtonPressedArgs {
                Direction = Vector2.down
            });
        }

        else if (Input.GetKeyDown(KeyCode.A)) {
            OnMovementButtonPressed?.Invoke(this, new OnMovementButtonPressedArgs {
                Direction = Vector2.left
            });
        }

        else if (Input.GetKeyDown(KeyCode.D)) {
            OnMovementButtonPressed?.Invoke(this, new OnMovementButtonPressedArgs {
                Direction = Vector2.right
            });
        }

    }
}
