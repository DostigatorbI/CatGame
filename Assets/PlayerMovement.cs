using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private PlayerInput _playerInput;
    private CharacterController _controller;
    private Vector3 _moveDirection;
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
    [SerializeField] float _fallGravityMultiplier;
    [SerializeField] float _jumpVelocity;
    [SerializeField] float _speed;
    private bool _isJumpButtonPressed;
    private bool _isJumping = false;
    private float _fallGravity;
    private float _gravityScale;


    // Start is called before the first frame update
    void Awake()
    {
        _controller = null;
        _playerInput = new PlayerInput();
        TryGetComponent<CharacterController>(out _controller);
        TryGetComponent<Rigidbody>(out _playerRigidBody);
        _fallGravity = _gravity * _fallGravityMultiplier;
        _gravityScale = _gravity;
    }

    private void OnEnable()
    {
        _playerInput.Enable();
        _playerInput.Movement.Move.started += onMovementInput;
        _playerInput.Movement.Move.canceled += onMovementInput;
        //_playerInput.Movement.Jump.started += onJump;
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
        // if(_moveDirection.y > 0.0f && _isJumping)
        // {
        //     Debug.Log("Rising");
        //     if(!_isJumpButtonPressed)
        //     {
        //         _gravityScale = _fallGravity;
        //     }
        // }
        // else if(_moveDirection.y < 0.0f && _isJumping){
        //     Debug.Log("Descending");
        //     _gravityScale = _fallGravity;
        // }

        // if(!_isJumping && _isJumpButtonPressed && _controller.isGrounded)
        // {
        //     _isJumping = true;
        //     _moveDirection.y = _jumpVelocity;
        // }
        // else if(!_isJumpButtonPressed && _isJumping && _controller.isGrounded)
        // {
        //     _isJumping = false;
        //     _gravityScale = _gravity;
        // }

        if(_isJumpButtonPressed && _isGrounded)
        {
            _playerRigidBody.AddForce(Vector3.up * _jumpVelocity, ForceMode.Impulse);
        }

    }
    private void onMovementInput(InputAction.CallbackContext context)
    {
        _move.x = context.ReadValue<Vector2>().x;
       //_moveDirection.x = context.ReadValue<Vector2>().x * _speed;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        //_moveDirection.y += Physics.gravity.y * _gravityScale * Time.deltaTime;
        //_controller.Move(_moveDirection * Time.deltaTime);
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
