using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

[WorldSystemFilter(WorldSystemFilterFlags.LocalSimulation)]
public partial struct PlayerMoveSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        
        // 1. 入力の取得 (WASD)
        float horizontal = 0;
        float vertical = 0;

        var keyboard = Keyboard.current;
        if (keyboard == null)
        {
            Debug.Log("キーボード取れてない");
            return;
        }

        
        if (keyboard == null) return;

        bool qPressed = keyboard.qKey.wasPressedThisFrame;
        if (keyboard.wKey.isPressed) vertical += 1;
        if (keyboard.sKey.isPressed) vertical -= 1;
        if (keyboard.dKey.isPressed) horizontal += 1;
        if (keyboard.aKey.isPressed) horizontal -= 1;

        float2 input = new float2(horizontal, vertical);

        // 入力がない場合は何もしない
        if (math.all(input == float2.zero)) return;

        // 2. 斜め移動でも速度が変わらないように正規化
        float2 direction = math.normalize(input);

        float deltaTime = SystemAPI.Time.DeltaTime;

        // 3. 全てのプレイヤーEntityに対して移動処理を行う
        foreach (var (velocity, transform, animState, playerState) in
                 SystemAPI.Query<RefRW<PhysicsVelocity>, RefRW<LocalTransform>, RefRW<PlayerAnimationState>, RefRW<PlayerState>>().WithAll<PlayerTag>())
        {
            // Qキーでモード切り替え
            if (qPressed)
            {
                playerState.ValueRW.IsRuning = !playerState.ValueRO.IsRuning;
            }

            if (math.any(input))
            {
                // ここで再宣言(float2)せず、計算した値を使います
                float2 moveDir = math.normalize(input);
                float targetSpeed = playerState.ValueRO.IsRuning ? playerState.ValueRO.RunSpeed : playerState.ValueRO.WalkSpeed;

                // 速度代入
                velocity.ValueRW.Linear.xz = moveDir * targetSpeed;

                velocity.ValueRW.Angular = Unity.Mathematics.float3.zero;
                //プレイヤーの向き
                float3 targetDirection = new float3(direction.x, 0, direction.y);
                quaternion targetRotation = quaternion.LookRotationSafe(targetDirection, math.up());
                float rotationSpeed = 10f; // 回転速度
                transform.ValueRW.Rotation = math.slerp(transform.ValueRO.Rotation, targetRotation, deltaTime * rotationSpeed);

                // アニメーション状態
                animState.ValueRW.CurrentState = playerState.ValueRO.IsRuning ? PlayerAnimateStateType.Run : PlayerAnimateStateType.Walk;
            }
            else
            {
                // 入力がない時は即停止
                velocity.ValueRW.Linear.xz = float2.zero;
                animState.ValueRW.CurrentState = PlayerAnimateStateType.Idle;
            }
            

        }
    }
}
