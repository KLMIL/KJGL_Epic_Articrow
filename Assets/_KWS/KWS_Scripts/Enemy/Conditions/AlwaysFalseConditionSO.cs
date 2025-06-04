using UnityEngine;

[CreateAssetMenu(fileName = "AlwaysFalseCondition", menuName = "Enemy/Condition/Always False")]
public class AlwaysFalseConditionSO : EnemyConditionSO
{
    public override bool IsMet(EnemyController controller) => false;
}
