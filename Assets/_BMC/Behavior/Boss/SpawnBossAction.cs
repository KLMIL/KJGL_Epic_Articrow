using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "SpawnBoss", story: "Spawn [Self]", category: "Action", id: "f00d65e1d972568654d8c63514c97c51")]
public partial class SpawnBossAction : Action
{
    Animator _anim;
    float _animWaitTime;
    [SerializeReference] public BlackboardVariable<GameObject> Self;

    protected override Status OnStart()
    {
        _anim = Self.Value.GetComponent<Animator>();
        _anim.Play("Spawn");
        return Status.Running;
    }
}