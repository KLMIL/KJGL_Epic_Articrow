using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using BMC;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "EnableRushDirectionVisual", story: "[Self] [IsCanRush] visual rush direction to [Target]", category: "Action", id: "317c3e77d1e293456a64eb5948a17bb7")]
public partial class EnableRushDirectionVisualAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<GameObject> Target;

    BossFSM _fsm;

    protected override Status OnStart()
    {
        if(_fsm == null)
            _fsm = Self.Value.GetComponent<BossFSM>();

        _fsm.Anim.Play("ReadyRush");

        Debug.Log("돌진 전조 on");
        return Status.Running;
    }
}