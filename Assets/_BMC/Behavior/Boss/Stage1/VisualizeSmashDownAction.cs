using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using BMC;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "VisualizeSmashDownAction", story: "[BossFSM] visualize smash down during [WaitSmashTime]", category: "Action", id: "8f984819e0bf61743e5d6ffd7c59f96e")]
public partial class VisualizeSmashDownAction : Action
{
    [SerializeReference] public BlackboardVariable<BossFSM> BossFSM;
    [SerializeReference] public BlackboardVariable<float> WaitSmashTime;
    ShokeWaveAttackIndicator _enemyAttackIndicator;

    protected override Status OnStart()
    {
        if (_enemyAttackIndicator == null)
        {
            _enemyAttackIndicator = BossFSM.Value.GetComponentInChildren<ShokeWaveAttackIndicator>();
        }

        _enemyAttackIndicator.Init(WaitSmashTime.Value);
        _enemyAttackIndicator.PlayChargeAndAttack();
        BossFSM.Value.Anim.Play("ReadySmash");

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