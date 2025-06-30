using BMC;
using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "BossChase", story: "[GolemFSM] Chase [Target]", category: "Action", id: "912bc856e39d50b808355dbf63508aae")]
public partial class BossChaseAction : Action
{
    [SerializeReference] public BlackboardVariable<GolemFSM> GolemFSM;
    [SerializeReference] public BlackboardVariable<GameObject> Target;
    float _speed = 3f;
    Vector2 _direction;

    protected override Status OnStart()
    {
        GolemFSM.Value.NavMeshAgent.enabled = true;
        GolemFSM.Value.NavMeshAgent.speed = _speed;
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        GolemFSM.Value.NavMeshAgent.SetDestination(Target.Value.transform.position);
        _direction = Target.Value.transform.position - GolemFSM.Value.transform.position;
        GolemFSM.Value.FlipX(_direction.x);
        return Status.Running;
    }

    protected override void OnEnd()
    {
        GolemFSM.Value.NavMeshAgent.enabled = false;
    }
}