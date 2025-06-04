using JetBrains.Annotations;
using UnityEngine;

[CreateAssetMenu(fileName = "ReceivedDamageCondition", menuName = "Enemy/Condition/Received Damage")]
public class ReceivedDamageConditionSO : EnemyConditionSO
{
    public override bool IsMet(EnemyController controller)
    {
        return controller.isDamaged;
    }
}
