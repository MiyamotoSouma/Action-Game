//using UnityEngine;
//using UnityEngine.Playables;

//public class PlayerComb : MonoBehaviour
//{
//    private PlayerData data;
//    private PlayerAnimator anim;
//    [SerializeField] private PlayableDirector director;

//    private static readonly int ComboIndex = Animator.StringToHash("ComboIndex");
//    private static readonly int AttackAnim = Animator.StringToHash("Attack");

//    public void Setup(PlayerData data, PlayerAnimator anim)
//    {
//        this.data = data;
//        this.anim = anim;
//    }

//    public void ProcessAttack()
//    {
//        int prevStep = data.ComboStep;
//        data.OnAttackInput();

//        if (prevStep == 0 && data.ComboStep == 1)
//        {
//            // 初段：アニメーション再生開始
//            anim.SetInteger(ComboIndex, 1);
//            anim.CrossFade(AttackAnim, 0.1f);
//            if (director != null) director.Play();
//        }
//    }


//    public void Sig_OpenWindow() => data.OpenComboWindow();
//    public void Sig_CloseWindow() => data.CloseComboWindow();

//    public void Sig_CheckNextCombo()
//    {
//        if (data.HasNextComboQueued)
//        {
//            data.AdvanceCombo();
//            anim.SetInteger(ComboIndex, data.ComboStep);
//        }
//    }

//    public void Sig_OnAttackEnd() => data.NotifyAttackEnd();
//}

