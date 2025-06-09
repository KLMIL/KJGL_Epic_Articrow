using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "CheckTargetDetect", story: "Compare values of [CurrentDistance] and [ChaseDistance]", category: "Conditions", id: "3438390494523527aef51bbf09683468")]
public partial class CheckTargetDetectCondition : Condition
{
    [SerializeReference] public BlackboardVariable<float> CurrentDistance;
    [SerializeReference] public BlackboardVariable<float> ChaseDistance;

    // 조건의 참, 거짓 여부를 반환하는 메서드
    public override bool IsTrue()
    {
        if(CurrentDistance.Value <= ChaseDistance.Value)
        {
            return true;
        }

        return false;
    }
}