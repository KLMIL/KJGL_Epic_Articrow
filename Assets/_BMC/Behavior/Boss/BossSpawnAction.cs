using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "BossSpawn", story: "Spawn [Self]", category: "Action", id: "f00d65e1d972568654d8c63514c97c51")]
public partial class BossSpawnAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    Animator _anim;
    protected override Status OnStart()
    {
        if(_anim == null)
            _anim = Self.Value.GetComponent<Animator>();
        _anim.Play("Spawn");
        return Status.Running;
    }
}