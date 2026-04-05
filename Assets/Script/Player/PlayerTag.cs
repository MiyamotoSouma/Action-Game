using Unity.Entities;
using Unity.Mathematics;

// プレイヤーであることを示すタグと、移動速度のデータ
public struct PlayerTag : IComponentData { }

public struct PlayerState : IComponentData
{
    public float RunSpeed;
    public float WalkSpeed;
    public bool IsRuning;
}

public struct PlayerAnimationState : IComponentData
{
    public float Speed;
    public PlayerAnimateStateType CurrentState;
    public PlayerAnimateStateType PreviousState;
}

