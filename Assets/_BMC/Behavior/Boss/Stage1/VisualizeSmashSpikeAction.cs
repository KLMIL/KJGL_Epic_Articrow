using BMC;
using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "VisualizeSmashSpike", story: "[BossFSM] visualize smash spike during [WaitSmashTime]", category: "Action", id: "e422094d889606259ae1a7ffc08c6db0")]
public partial class VisualizeSmashSpikeAction : Action
{
    [SerializeReference] public BlackboardVariable<BossFSM> BossFSM;
    [SerializeReference] public BlackboardVariable<float> WaitSmashTime;
    SpikeAttackIndicator _spikeAttackIndicator;

    protected override Status OnStart()
    {
        if(_spikeAttackIndicator == null)
        {
            _spikeAttackIndicator = BossFSM.Value.GetComponentInChildren<SpikeAttackIndicator>();
        }

        _spikeAttackIndicator.Init(WaitSmashTime.Value);
        _spikeAttackIndicator.PlayChargeAndAttack();
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