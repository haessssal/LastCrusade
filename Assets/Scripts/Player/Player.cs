using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerStats))]
public class Player : MonoBehaviour
{
    [Header("References")]
    public Animator animator;
    public Transform crusader;

    private Rigidbody2D rb;
    private PlayerStats stats;

    [Header("Ground Check")]
    public Transform groundCheck;
    public LayerMask groundLayer;
    public float groundCheckRadius = 0.2f;

    private float moveInput;
    private PlayerStateMachine stateMachine;

    // States
    [HideInInspector] public PlayerIdleState idleState;
    [HideInInspector] public PlayerWalkState walkState;
    [HideInInspector] public PlayerJumpState jumpState;

    // Input System
    public PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction jumpAction;

    public Rigidbody2D RB => rb;
    public PlayerStats Stats => stats;
    public bool IsGrounded { get; private set; }

    public void Initialize(PlayerStats playerStats)
    {
        stats = playerStats;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        stats = GetComponent<PlayerStats>();

        stateMachine = new PlayerStateMachine();

        idleState = new PlayerIdleState(this, stateMachine, animator);
        walkState = new PlayerWalkState(this, stateMachine, animator);
        jumpState = new PlayerJumpState(this, stateMachine, animator);
    }

    private void Start()
    {
        if (playerInput != null)
        {
            foreach (var map in playerInput.actions.actionMaps) map.Disable();

            // 기본 Enable
            var currentMap = playerInput.currentActionMap;
            if (currentMap != null)
            {
                currentMap.Enable();
                Debug.Log($"{gameObject.name} Enabled ActionMap: {currentMap.name}");
            }
        }

        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];
        stateMachine.Initialize(idleState);
    }

    private void Update()
    {
        // 땅 체크
        IsGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // 상태 실행
        stateMachine.CurrentState.Execute();
    }

    public Vector2 GetMoveInput() => moveAction.ReadValue<Vector2>();   
    public bool IsJumpPressed() => jumpAction.WasPressedThisFrame();

    public void Move(float dir)
    {
        rb.linearVelocity = new Vector2(dir * stats.moveSpeed, rb.linearVelocity.y);

        // if (dir > 0) crusader.localScale = new Vector3(1, 1, 1);
        // else if (dir < 0) crusader.localScale = new Vector3(-1, 1, 1);
        if (dir > 0) transform.localScale = new Vector3(0.27f, 0.27f, 1);
        else if (dir < 0) transform.localScale = new Vector3(-0.27f, 0.27f, 1);
    }

    public void Jump(float force)
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, force);
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}