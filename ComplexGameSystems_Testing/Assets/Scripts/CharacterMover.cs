using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class CharacterMover : MonoBehaviour
{
    // Set on awake
    private PlayerInput _playerInput;
    private CharacterController _cc;
    private Camera _camera;

    // Set in inspector
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;

    // This will contain basic controls based on our key inputs
    private Vector2 _direction;
    private float _speed;
    private bool _isMoving;
    private bool _isSprinting;
    private bool _isJumping;

    // Private fields
    private Vector2 _playerVelocity;
    private Vector2 _smoothPlayerVelocity;
    private bool _isGrounded = false;
    private Vector3 _hitDirection = new Vector3();

    // Public fields
    public float movementSpeed = 4f;
    public float sprintingSpeed = 10f;
    public float jumpHeight = 2f;
    public float smoothTime = 1f;
    public float rotationSpeed = 10f;

    public Vector3 velocity = new Vector3();

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _cc = GetComponent<CharacterController>();
        _camera = Camera.main;
    }

    public void OnMove(InputAction.CallbackContext value)
    {
        _direction = value.ReadValue<Vector2>();
        if (_direction == Vector2.zero)
            _isMoving = false;
        else
            _isMoving = true;
    }

    public void OnSprint(InputAction.CallbackContext value)
    {
        if (value.started)
            _isSprinting = true;
        else if (value.canceled)
            _isSprinting = false;
    }

    public void OnJump(InputAction.CallbackContext value)
    {
        if (value.started)
            _isJumping = true;
        else if (value.canceled)
            _isJumping = false;
    }

    public void OnLook(InputAction.CallbackContext value)
    {
        Quaternion rotation = Quaternion.Euler(0, _camera.transform.eulerAngles.y, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.fixedDeltaTime * rotationSpeed);
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        _speed = !_isMoving ? 0 : _isSprinting ? sprintingSpeed : movementSpeed;
        _playerVelocity = Vector2.SmoothDamp(_playerVelocity, _direction, ref _smoothPlayerVelocity, smoothTime);

        Vector3 camForward = _camera.transform.forward;
        camForward.y = 0;
        camForward.Normalize();

        Vector3 movedelta = (_playerVelocity.x * _camera.transform.right + _playerVelocity.y * camForward) * _speed;

        velocity.x = movedelta.x;
        velocity.z = movedelta.z;

        // Check for jumping
        if (_isJumping && OnGround())
            velocity.y = Mathf.Sqrt(-2 * Physics.gravity.y * jumpHeight);

        // Check if we've hit ground from falling. If so, remove our velocity
        if (_isGrounded && velocity.y < 0)
            velocity.y = 0;

        // Apply gravity
        velocity += Physics.gravity * Time.fixedDeltaTime;

        if (!_isGrounded)
            _hitDirection = Vector3.zero;

        // Slide objects off surfaces they're hanging on to
        if (_playerVelocity.x == 0 && _playerVelocity.y == 0)
        {
            Vector3 horizontalHitDirection = _hitDirection;
            horizontalHitDirection.y = 0;
            float displacment = horizontalHitDirection.magnitude;
            if (displacment > 0.3f)
                velocity -= 0.2f * horizontalHitDirection / displacment;
        }

        velocity += GetGroundVelocity();

        _cc.Move(velocity * Time.fixedDeltaTime);
        _isGrounded = _cc.isGrounded;

        Quaternion rotation = Quaternion.Euler(0, _camera.transform.eulerAngles.y, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.fixedDeltaTime * rotationSpeed);
    }

    private Vector3 GetGroundVelocity()
    {
        RaycastHit hit;
        Debug.DrawRay(transform.position, -transform.up * 50, Color.red, 10);
        if (Physics.Raycast(transform.position, -transform.up, out hit, 1))
        {
            if (hit.rigidbody)
                return hit.rigidbody.velocity;
        }

        return Vector3.zero;
    }

    /// <summary> Checks if the character is on the ground. Using the Character Controller's isGrounded and a raycast pointing down. </summary>
    private bool OnGround()
    {
        return _cc.isGrounded || Physics.Raycast(transform.position, -transform.up, .4f);
    }
}
