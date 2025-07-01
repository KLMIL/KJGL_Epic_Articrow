using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using BMC;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "BossIdle", story: "[BossFSM] Idle with looking at [Target]", category: "Action", id: "cd9a262eb69bd4eb3463e622b09ea720")]
public partial class BossIdleAction : Action
{
    [SerializeReference] public BlackboardVariable<BossFSM> BossFSM;
    [SerializeReference] public BlackboardVariable<GameObject> Target;
    Vector2 _lookDirection;

    protected override Status OnStart()
    {
        AnimatorStateInfo animatorStateInfo = BossFSM.Value.Anim.GetCurrentAnimatorStateInfo(0);
        if(!animatorStateInfo.IsName("Idle"))
        {
            BossFSM.Value.Anim.Play("Idle");
        }

        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        _lookDirection = (Target.Value.transform.position - BossFSM.Value.transform.position).normalized;
        BossFSM.Value.FlipX(_lookDirection.x);
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}