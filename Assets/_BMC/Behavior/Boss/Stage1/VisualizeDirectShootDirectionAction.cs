using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using BMC;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "VisualizeDirectShootDirection", story: "[Self] visualize direct shoot to [Target] during [WaitDirectShootTime]", category: "Action", id: "8a06ac02ae2e4012a47dc5d82738a144")]
public partial class VisualizeDirectShootDirectionAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<GameObject> Target;
    [SerializeReference] public BlackboardVariable<float> WaitDirectShootTime;

    BossFSM _fsm;
    LaserAttackIndicator _enemyAttackIndicator;
    Vector2 _lookDirection;
    float _readyTime = 0.25f;
    protected override Status OnStart()
    {
        if (_fsm == null)
        {
            _fsm = Self.Value.GetComponent<BossFSM>();
            _enemyAttackIndicator = Self.Value.GetComponentInChildren<LaserAttackIndicator>();
        }

        _lookDirection = (Target.Value.transform.position - Self.Value.transform.position).normalized;
        _fsm.FlipX(_lookDirection.x);
        _enemyAttackIndicator.Init(_readyTime);
        _enemyAttackIndicator.PlayChargeAndAttack();
        _fsm.Anim.Play("ReadyShoot");
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}