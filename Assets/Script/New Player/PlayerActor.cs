using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerActor : MonoBehaviour
{
    [SerializeField] float walkSpeed = 5f;
    [SerializeField] float runSpeed = 8f;
    [SerializeField] float acceleration = 0.3f;
    [SerializeField] float deceleration = 0.5f;
    [SerializeField] float rotSpeed = 10f;
    [SerializeField] float jumpPower = 5f;

    float inputH;
    float inputV;
    bool jumpRequested;
    bool qKeyRequested;
    bool isGrounded;

    PlayerAnimType lastAnim = PlayerAnimType.Idle;

    static readonly int IdleAnim = Animator.StringToHash("Idle");
    static readonly int WalkAnim = Animator.StringToHash("Walk");
    static readonly int RunAnim = Animator.StringToHash("Run");
    static readonly int JumpAnim = Animator.StringToHash("Jump");
    static readonly int VertJumpAnim = Animator.StringToHash("VerticlJump");
    static readonly int AttackAnim = Animator.StringToHash("Attack");

    PlayerAnimator anim;
    Rigidbody rb;
    PlayerData data;
    PlayerMover mover;
    //PlayerComb combat;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<PlayerAnimator>();

        data = new PlayerData(walkSpeed, runSpeed, acceleration, deceleration, rotSpeed, jumpPower);

        mover = gameObject.AddComponent<PlayerMover>();
        mover.Setup(data, rb);

        //combat = gameObject.AddComponent<PlayerComb>();
        //combat.Setup(data, anim);
    }

    void Update()
    {
        GroundCheck();

        var kb = Keyboard.current;
        var mouse = Mouse.current;
        if (kb == null || mouse == null) return;

        inputH = (kb.dKey.isPressed ? 1 : 0) - (kb.aKey.isPressed ? 1 : 0);
        inputV = (kb.wKey.isPressed ? 1 : 0) - (kb.sKey.isPressed ? 1 : 0);

        if (kb.qKey.wasPressedThisFrame)
            qKeyRequested = true;

        if (kb.spaceKey.wasPressedThisFrame && data.CanJump(isGrounded))
            jumpRequested = true;

        if (mouse.leftButton.wasPressedThisFrame)
            data.OnAttackInput();

        anim.UpdateState(data.P_anim);
    }

    void FixedUpdate()
    {
        if (qKeyRequested)
        {
            data.ToggleRunMode(inputH != 0 || inputV != 0);
            qKeyRequested = false;
        }

        mover.Move(inputH, inputV, Keyboard.current.leftShiftKey.isPressed, isGrounded, jumpRequested);

        jumpRequested = false;
    }

    void GroundCheck()
    {
        isGrounded = Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, 0.2f);
    }

}