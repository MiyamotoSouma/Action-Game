using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    Animator anim;

    static readonly int Idle = Animator.StringToHash("Idle");
    static readonly int Walk = Animator.StringToHash("Walk");
    static readonly int Run = Animator.StringToHash("Run");
    static readonly int Jump = Animator.StringToHash("Jump");
    static readonly int VertJump = Animator.StringToHash("VerticlJump");
    static readonly int Attack = Animator.StringToHash("Attack");
    static readonly int ComboIndex = Animator.StringToHash("ComboIndex");

    PlayerAnimType lastState = PlayerAnimType.Idle;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void UpdateState(PlayerAnimType state)
    {
        if (state == lastState) return;

        int hash = state switch
        {
            PlayerAnimType.Idle => Idle,
            PlayerAnimType.Walk => Walk,
            PlayerAnimType.Run => Run,
            PlayerAnimType.Jump => Jump,
            PlayerAnimType.VerticlJump => VertJump,
            PlayerAnimType.Attack => Attack,
            _ => Idle
        };

        anim.CrossFade(hash, 0.1f);
        lastState = state;
    }

    public void SetCombo(int step)
    {
        anim.SetInteger(ComboIndex, step);
    }
}