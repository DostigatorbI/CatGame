using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private PlayerInput _playerInput;
    private Vector3 _move;
    private float _maxForce = 1.0f;

    [SerializeField]private bool _isGrounded;
    public bool IsGrounded
    {
        get{return _isGrounded;}
        set{_isGrounded = value;}
    }

    private Rigidbody _playerRigidBody;
    [SerializeField] float _gravity;
    [SerializeField] float _jumpVelocity;
    [SerializeField] float _speed;
    private bool _isJumpButtonPressed;
    private bool _isJumping = false;


    void Awake()
    {
        _playerInput = new PlayerInput();
        TryGetComponent<Rigidbody>(out _playerRigidBody);
    }

    private void OnEnable()
    {
        _playerInput.Enable();
        _playerInput.Movement.Move.started += onMovementInput;
        _playerInput.Movement.Move.canceled += onMovementInput;
        _playerInput.Movement.Jump.started += ctx=>{_isJumpButtonPressed = true;};
        _playerInput.Movement.Jump.canceled += ctx=> {_isJumpButtonPressed = false;}; 
    }

    private void OnDisable()
    {
        _playerInput.Movement.Move.started -= onMovementInput;
        _playerInput.Movement.Move.canceled -= onMovementInput;
        _playerInput.Movement.Jump.started -= ctx=>{_isJumpButtonPressed = true;};
        _playerInput.Movement.Jump.canceled -= ctx=> {_isJumpButtonPressed = false;}; 
        _playerInput.Disable();
    }

    private void JumpHandler()
    {
        if(!_isJumping && _isJumpButtonPressed && _isGrounded)
        {
            _isJumping = true;
            _playerRigidBody.AddForce(Vector3.up * _jumpVelocity, ForceMode.Impulse);
        }
        else if(!_isJumpButtonPressed && _isJumping && _isGrounded)
        {
            _isJumping = false;
        }
        if(_playerRigidBody.velocity.y > 0 && _isJumping)
        {
            Debug.Log("Up");
            if(!_isJumpButtonPressed)
            {
                _playerRigidBody.AddForce(Vector3.down * _gravity);
            }
        }
        else if(_playerRigidBody.velocity.y < 0){
            _playerRigidBody.AddForce(Vector3.down * _gravity);
            Debug.Log("Down " + _playerRigidBody.velocity);
        }

    }
    private void onMovementInput(InputAction.CallbackContext context)
    {
        _move.x = context.ReadValue<Vector2>().x;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
        JumpHandler();
    }

    private void Move()
    {
        Vector3 currentVelocity = _playerRigidBody.velocity;
        Vector3 targetVelocity = new Vector3(_move.x,0,0);
        targetVelocity *= _speed;
        targetVelocity = transform.TransformDirection(targetVelocity);
        Vector3 VelocityChange = targetVelocity - currentVelocity;
        VelocityChange = new Vector3(VelocityChange.x, 0, VelocityChange.z);
        Vector3.ClampMagnitude(VelocityChange, _maxForce);
        _playerRigidBody.AddForce(VelocityChange, ForceMode.VelocityChange);
    }
}
