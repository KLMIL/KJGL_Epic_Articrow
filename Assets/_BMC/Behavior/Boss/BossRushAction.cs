using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using BMC;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "BossRush", story: "[BossFSM] rush towards [Target] in a [RushDirection] with [IsCollisionWithObstacle]", category: "Action", id: "05761265047ab4ff5c4f4a0a8d2489b1")]
public partial class BossRushAction : Action
{
    [SerializeReference] public BlackboardVariable<BossFSM> BossFSM;
    [SerializeReference] public BlackboardVariable<GameObject> Target;
    [SerializeReference] public BlackboardVariable<Vector2> RushDirection;
    [SerializeReference] public BlackboardVariable<bool> IsCollisionWithObstacle;
    float _rushForce = 12000f;

    protected override Status OnStart()
    {
        BossFSM.Value.HitBox.OnOff(true);
        if (Target.Value != null && BossFSM.Value != null)
        {
            Debug.Log("돌진 방향 계산");
            BossFSM.Value.Anim.Play("Rush");
            BossFSM.Value.FlipX(RushDirection.Value.x);
        }
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        //_lastVelocity = _rb.linearVelocity;

        if (IsCollisionWithObstacle.Value)
        {
            Debug.Log("충돌 감지되어 반사해야함");
            IsCollisionWithObstacle.Value = false;
            //float force = _rb.linearVelocity.magnitude;
            //_rb.linearVelocity = RushDirection.Value * force;
            BossFSM.Value.RB.AddForce(RushDirection.Value * _rushForce * Time.deltaTime, ForceMode2D.Force);
        }
        else
        {
            // 해당 방향으로 직진
            BossFSM.Value.RB.AddForce(RushDirection.Value * _rushForce * Time.deltaTime, ForceMode2D.Force);
        }

        return Status.Running;
    }

    protected override void OnEnd()
    {
        BossFSM.Value.HitBox.OnOff(false);
    }
}