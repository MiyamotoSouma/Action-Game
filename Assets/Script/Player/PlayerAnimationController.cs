using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

[DisallowMultipleComponent]
public class PlayerAnimationController : MonoBehaviour
{
    private Animator animator;
    private EntityManager entityManager;
    private Entity entity;

    void Start()
    {
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        if (World.DefaultGameObjectInjectionWorld == null) return;
        if (entityManager == default)
            entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

        if (entity == Entity.Null)
        {
            var query = entityManager.CreateEntityQuery(typeof(PlayerTag));
            if (query.HasSingleton<PlayerTag>())
            {
                entity = query.GetSingletonEntity();
                Debug.Log("Entityが見つかりました！同期を開始します。");
            }
            return;
        }

  
        // --- 2. アニメーションの制御 (既存) ---
        if (entityManager.HasComponent<PlayerAnimationState>(entity))
        {
            var animData = entityManager.GetComponentData<PlayerAnimationState>(entity);

            if (animData.CurrentState != animData.PreviousState)
            {
                Debug.Log($"State Changed to: {animData.CurrentState}");
                animator.CrossFade(animData.CurrentState.ToString(), 0.1f);

                animData.PreviousState = animData.CurrentState;
                entityManager.SetComponentData(entity, animData);
            }
        }
    }
}

