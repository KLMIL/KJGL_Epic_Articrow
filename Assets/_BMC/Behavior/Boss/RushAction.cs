using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine.Rendering;
using static UnityEngine.RuleTile.TilingRuleOutput;
using UnityEngine.EventSystems;
using BMC;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Rush", story: "[Self] rush towards [Target] in a [RushDirection] with [IsCollisionWithObstacle]", category: "Action", id: "05761265047ab4ff5c4f4a0a8d2489b1")]
public partial class RushAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<GameObject> Target;
    [SerializeReference] public BlackboardVariable<Vector2> RushDirection;
    [SerializeReference] public BlackboardVariable<bool> IsCollisionWithObstacle;

    BossController _bossController;

    Rigidbody2D _rb;
    float _rushForce = 25f;
    public LayerMask ObstacleLayer;          // 장애물 레이어

    protected override Status OnStart()
    {
        if(_bossController == null)
            _bossController = Self.Value.GetComponent<BossController>();

        if (RushDirection.Value == Vector2.zero && Target != null && Self != null)
        {
            Debug.Log("돌진 방향 계산");
            _rb = Self.Value.GetComponent<Rigidbody2D>();
            RushDirection.Value = (Target.Value.transform.position - Self.Value.transform.position).normalized;
            _bossController.RushDirection = RushDirection.Value;
        }
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        //_lastVelocity = _rb.linearVelocity;

        if(IsCollisionWithObstacle.Value)
        {
            Debug.Log("충돌 감지되어 반사해야함");
            IsCollisionWithObstacle.Value = false;
            //float force = _rb.linearVelocity.magnitude;
            //_rb.linearVelocity = RushDirection.Value * force;
            _rb.AddForce(RushDirection.Value * _rushForce, ForceMode2D.Force);
        }
        else
        {
            // 해당 방향으로 직진
            // Self.Value.transform.Translate(RushDirection.Value * _rushSpeed * Time.deltaTime);
            _rb.AddForce(RushDirection.Value * _rushForce, ForceMode2D.Force);
        }

        return Status.Success;
    }
}