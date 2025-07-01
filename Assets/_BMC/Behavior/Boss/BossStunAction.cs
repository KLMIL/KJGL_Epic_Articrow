using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using BMC;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "BossStunAction", story: "[BossFSM] Stun", category: "Action", id: "b26c4a10d14a11afd59da5b6649b236d")]
public partial class BossStunAction : Action
{
    [SerializeReference] public BlackboardVariable<BossFSM> BossFSM;

    protected override Status OnStart()
    {
        BossFSM.Value.RB.linearVelocity = Vector2.zero; // 보스의 속도를 0으로 초기화
        BossFSM.Value.Anim.Play("Stun");

        return Status.Running;
    }

    //protected override Status OnUpdate()
    //{
    //    return Status.Success;
    //}

    //protected override void OnEnd()
    //{
    //}
}