using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "CheckObstacleInRushDirection", story: "[Self] Check Obstacle In [RushDirection]", category: "Conditions", id: "8e5af1a9185b5d5cbd1c96839e1637c2")]
public partial class CheckObstacleInRushDirectionCondition : Condition
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<Vector2> RushDirection;

    public override bool IsTrue()
    {
        RaycastHit2D hit = Physics2D.Raycast(Self.Value.transform.position, RushDirection.Value, 1f, LayerMask.GetMask("Obstacle"));
        
        if (hit != null)
        {
            //Debug.LogWarning($"벽에 닿았다고~{hit.collider.name}");
            Debug.DrawRay(Self.Value.transform.position, RushDirection.Value, Color.red, 1f);
        }

        return (hit != null) ? true : false;
    }

    public override void OnStart()
    {
    }

    public override void OnEnd()
    {
    }
}
