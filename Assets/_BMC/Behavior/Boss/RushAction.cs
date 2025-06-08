using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine.Rendering;
using static UnityEngine.RuleTile.TilingRuleOutput;
using UnityEngine.EventSystems;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Rush", story: "[Self] rush towards [Target] in a [RushDirection]", category: "Action", id: "05761265047ab4ff5c4f4a0a8d2489b1")]
public partial class RushAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<GameObject> Target;
    [SerializeReference] public BlackboardVariable<Vector2> RushDirection;

    Rigidbody2D _rb;
    float _rushForce = 3000f;
    public LayerMask ObstacleLayer;          // 장애물 레이어

    Vector3 _lastVelocity;

    protected override Status OnStart()
    {
        if (RushDirection.Value == Vector2.zero && Target != null && Self != null)
        {
            Debug.Log("돌진 방향 계산");
            _rb = Self.Value.GetComponent<Rigidbody2D>();
            RushDirection.Value = (Target.Value.transform.position - Self.Value.transform.position).normalized;
            //_rb.AddForce(RushDirection.Value * _rushForce, ForceMode2D.Force);
        }
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        //_lastVelocity = _rb.linearVelocity;

        // 해당 방향으로 직진
        // Self.Value.transform.Translate(RushDirection.Value * _rushSpeed * Time.deltaTime);
        _rb.AddForce(RushDirection.Value * 25f, ForceMode2D.Force);
        return Status.Success;
    }
}