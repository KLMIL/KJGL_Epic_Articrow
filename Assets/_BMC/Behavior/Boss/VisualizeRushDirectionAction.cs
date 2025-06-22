using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using BMC;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "VisualizeRushDirection", story: "[Self] visualize [RushDirection] to [Target] during [WaitRushTime] with [IsCanRush]", category: "Action", id: "317c3e77d1e293456a64eb5948a17bb7")]
public partial class VisualizeRushDirectionAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<Vector2> RushDirection;
    [SerializeReference] public BlackboardVariable<GameObject> Target;
    [SerializeReference] public BlackboardVariable<float> WaitRushTime;
    [SerializeReference] public BlackboardVariable<bool> IsCanRush;
    BossFSM _fsm;
    EnemyAttackIndicator _enemyAttackIndicator;

    protected override Status OnStart()
    {
        if (_fsm == null)
        {
            _fsm = Self.Value.GetComponent<BossFSM>();
            _enemyAttackIndicator = Self.Value.GetComponentInChildren<EnemyAttackIndicator>();
        }
        _enemyAttackIndicator.Init(WaitRushTime.Value);
        _enemyAttackIndicator.PlayChargeAndAttack();
        RushDirection.Value = (Target.Value.transform.position - Self.Value.transform.position).normalized;
        _fsm.Anim.Play("ReadyRush");
        Debug.Log("돌진 전조 on");
        return Status.Running;
    }
}