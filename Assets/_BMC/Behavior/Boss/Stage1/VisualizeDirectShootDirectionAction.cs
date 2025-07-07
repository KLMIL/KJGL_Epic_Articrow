using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using BMC;
using YSJ;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "VisualizeDirectShootDirection", story: "[BossFSM] visualize direct shoot to [Target] during [WaitDirectShootTime]", category: "Action", id: "8a06ac02ae2e4012a47dc5d82738a144")]
public partial class VisualizeDirectShootDirectionAction : Action
{
    [SerializeReference] public BlackboardVariable<BossFSM> BossFSM;
    [SerializeReference] public BlackboardVariable<GameObject> Target;
    [SerializeReference] public BlackboardVariable<float> WaitDirectShootTime;
    LaserAttackIndicator _enemyAttackIndicator;
    Vector2 _lookDirection;
    float _readyTime = 0.5f;
    protected override Status OnStart()
    {
        if (_enemyAttackIndicator == null)
        {
            _enemyAttackIndicator = BossFSM.Value.GetComponentInChildren<LaserAttackIndicator>();
        }
        _lookDirection = (Target.Value.transform.position - BossFSM.Value.transform.position).normalized;
        BossFSM.Value.FlipX(_lookDirection.x);
        _enemyAttackIndicator.Init(_readyTime);
        _enemyAttackIndicator.PlayChargeAndAttack();
        BossFSM.Value.Anim.Play("DirectShoot");
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