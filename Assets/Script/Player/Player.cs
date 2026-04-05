using Unity.Entities;
using Unity.Physics;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float runSpeed = 10f;
    public float walkSpeed = 20f;
    private bool isRuning = false; 

    // このクラスがGameObjectにアタッチされる
    class Baker : Baker<Player>
    {
        public override void Bake(Player authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
      

            AddComponent<PlayerTag>(entity);
            AddComponent(entity, new PlayerState
            {
                RunSpeed = authoring.runSpeed,
                WalkSpeed = authoring.walkSpeed,
                IsRuning = authoring.isRuning
            });

            AddComponent(entity, new PlayerAnimationState
            {
                CurrentState = PlayerAnimateStateType.Idle,
                PreviousState = PlayerAnimateStateType.Idle
            });

        }
    }
}

