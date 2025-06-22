using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using BMC;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "BossHit", story: "[Self] hit by target", category: "Action", id: "61f427f47003c289c464918522b3740c")]
public partial class BossHitAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    GolemBossStatus _status;

    protected override Status OnStart()
    {
        if(_status == null)
            _status = Self.Value.GetComponent<GolemBossStatus>();

        _status.TakeDamage(10f);
        return Status.Running;
    }
}