using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerInput _input;

    private InputAction _move;
    private InputAction _interact;
    private InputAction _use;

    public static bool Interact { get; set; }

    [SerializeField]
    private float _moveSpeed;

    [SerializeField]
    private Rigidbody2D _rb2d;

    private void Awake()
    {
        _input = new PlayerInput();

        _move ??= _input.Player.Move;
        _interact ??= _input.Player.Interact;
        _use ??= _input.Player.Use;
    }

    private void OnEnable()
    {
        _move.Enable();
        _interact.Enable();
        _use.Enable();
    }

    private void OnDisable()
    {
        _move.Disable();
        _interact.Disable();
        _use.Disable();
    }

    private void Update()
    {
        Interact = _interact.WasPerformedThisFrame();
    }

    private void FixedUpdate()
    {
        Vector2 direction = _move.ReadValue<Vector2>();
        direction.x /= (direction.x != 0) ? Mathf.Abs(direction.x) : 1;
        direction.y /= (direction.y != 0) ? Mathf.Abs(direction.y) : 1;

        _rb2d.linearVelocity = direction * _moveSpeed;
    }
}
