using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "CheckCollisionWithWall", story: "Check [Agent] Collisions With Wall", category: "Conditions", id: "ff57490b79e02ec07433c79ed40e9ce7")]
public partial class CheckCollisionWithWallCondition : Condition
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;

    public override bool IsTrue()
    {
        Collider2D hit = Physics2D.OverlapCircle(Agent.Value.transform.position, 0.5f, LayerMask.GetMask("Obstacle"));
        return (hit != null) ? true : false;
    }
}