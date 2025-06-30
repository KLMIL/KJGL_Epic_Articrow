using BMC;
using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "PlayReadyAttackAnimation", story: "[BossFSM] play [AnimationName]", category: "Action", id: "e35962cf02be0c36f560d00418c27485")]
// 전조 애니메이션 재생 액션
public partial class PlayReadyAttackAnimationAction : Action
{
    [SerializeReference] public BlackboardVariable<BossFSM> BossFSM;
    [SerializeReference] public BlackboardVariable<string> AnimationName;

    protected override Status OnStart()
    {
        BossFSM.Value.Anim.Play(AnimationName.Value);
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