using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using BMC;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "BossStunAction", story: "[Self] Stun", category: "Action", id: "b26c4a10d14a11afd59da5b6649b236d")]
public partial class BossStunAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    BossFSM _fsm;

    protected override Status OnStart()
    {
        if (_fsm == null)
            _fsm = Self.Value.GetComponent<BossFSM>();

        _fsm.Anim.Play("Stun");

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