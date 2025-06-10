using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using BMC;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "BossSpawn", story: "[Self] Spawn", category: "Action", id: "f00d65e1d972568654d8c63514c97c51")]
public partial class BossSpawnAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    BossFSM _fsm;

    protected override Status OnStart()
    {
        if(_fsm == null)
            _fsm = Self.Value.GetComponent<BossFSM>();
        _fsm.Anim.Play("Spawn");
        return Status.Running;
    }
}