using JetBrains.Annotations;
using UnityEngine;

[CreateAssetMenu(fileName = "ReceivedDamageCondition", menuName = "Enemy/Condition/ReceivedDamage")]
public class ReceivedDamageConditionSO : EnemyConditionSO
{
    [HideInInspector] public bool hasTakenDamage = false;

    public override bool IsMet(EnemyController controller)
    {
        return hasTakenDamage;
    }

    public void OnDamage() => hasTakenDamage = true;
    public void ResetTrigger() => hasTakenDamage = false;
}
