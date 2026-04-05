using UnityEngine;

public enum PlayerAnimType
{
    Idle,
    Walk,
    Run,
    Jump,
    VerticlJump,
    Attack
}

public class PlayerData
{
    public float WalkSpeed { get; }
    public float RunSpeed { get; }
    public float Acceleration { get; }
    public float Deceleration { get; }
    public float RotationSpeed { get; }
    public float JumpPower { get; }

    public Vector3 Velocity { get; private set; } = Vector3.zero;
    Vector3 jumpInertia = Vector3.zero;

    public bool IsRunMode { get; private set; }
    public PlayerAnimType P_anim { get; private set; } = PlayerAnimType.Idle;

    // ƒRƒ“ƒ{
    public int ComboStep { get; private set; }
    public bool IsComboWindowOpen { get; private set; }
    public bool HasNextComboQueued { get; private set; }

    public bool IsAttacking => P_anim == PlayerAnimType.Attack;

    public PlayerData(float w, float r, float a, float d, float rot, float j)
    {
        WalkSpeed = w;
        RunSpeed = r;
        Acceleration = a;
        Deceleration = d;
        RotationSpeed = rot;
        JumpPower = j;
    }

    public bool CanJump(bool grounded)
    {
        return grounded && !IsAttacking;
    }

    public void OnAttackInput()
    {
        if (!IsAttacking)
        {
            ComboStep = 1;
            P_anim = PlayerAnimType.Attack;
        }
        else if (IsComboWindowOpen)
        {
            HasNextComboQueued = true;
        }
    }

    public void ToggleRunMode(bool moving)
    {
        if (moving) IsRunMode = !IsRunMode;
    }

    // ===== ˆÚ“® =====
    public Vector3 CalculateVelocity(float h, float v, bool shift, bool grounded, float dt)
    {
        if (IsAttacking)
        {
            Velocity = Vector3.zero;
            return Velocity;
        }

        Vector3 input = new Vector3(h, 0, v);

        if (grounded)
        {
            Vector3 target = Vector3.zero;

            if (input != Vector3.zero)
            {
                bool run = IsRunMode || shift;
                float speed = run ? RunSpeed : WalkSpeed;

                target = input.normalized * speed;
                P_anim = run ? PlayerAnimType.Run : PlayerAnimType.Walk;
            }
            else
            {
                P_anim = PlayerAnimType.Idle;
            }

            float rate = (target != Vector3.zero) ? Acceleration : Deceleration;

            Velocity = Vector3.Lerp(
                Velocity,
                target,
                1f - Mathf.Exp(-rate * 10f * dt)
            );

            jumpInertia = Velocity;
        }
        else
        {
            Velocity = jumpInertia;

            P_anim = jumpInertia.sqrMagnitude < 0.01f
                ? PlayerAnimType.VerticlJump
                : PlayerAnimType.Jump;
        }

        return Velocity;
    }

    public Quaternion CalculateRotation(Quaternion current, Vector3 input, float dt)
    {
        if (input.sqrMagnitude < 0.01f) return current;

        Quaternion target = Quaternion.LookRotation(input.normalized);
        return Quaternion.Slerp(current, target, RotationSpeed * dt);
    }
}