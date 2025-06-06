using UnityEngine;

[CreateAssetMenu(fileName = "FalseCondition", menuName = "Enemy/Condition/False")]
public class FalseCondition : EnemyConditionSO
{
    public override bool IsMet(EnemyController controller) => false;
}
