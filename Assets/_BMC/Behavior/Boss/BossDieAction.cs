using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using BMC;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "BossDie", story: "[BossFSM] Die", category: "Action", id: "caa60688413d516d464ea1a88d7a593e")]
public partial class BossDieAction : Action
{
    [SerializeReference] public BlackboardVariable<BossFSM> BossFSM;
    protected override Status OnStart()
    {
        BossFSM.Value.Anim.Play("Die");
        return Status.Running;
    }
}