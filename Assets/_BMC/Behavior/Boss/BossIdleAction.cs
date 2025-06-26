using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using BMC;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "BossIdle", story: "[Self] Idle with looking at [Target]", category: "Action", id: "cd9a262eb69bd4eb3463e622b09ea720")]
public partial class BossIdleAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<GameObject> Target;
    BossFSM _fsm;
    RushAttackIndicator _enemyAttackIndicator;
    Vector2 _lookDirection;

    protected override Status OnStart()
    {
        if(_fsm == null)
        {
            _fsm = Self.Value.GetComponent<BossFSM>();
            _enemyAttackIndicator = Self.Value.GetComponentInChildren<RushAttackIndicator>();
        }

        AnimatorStateInfo animatorStateInfo = _fsm.Anim.GetCurrentAnimatorStateInfo(0);
        if(!animatorStateInfo.IsName("Idle"))
        {
            _fsm.Anim.Play("Idle");
        }

        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        _lookDirection = (Target.Value.transform.position - Self.Value.transform.position).normalized;
        _fsm.FlipX(_lookDirection.x);
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}