using UnityEngine;

[CreateAssetMenu(fileName = "AlwaysTrueCondition", menuName = "Enemy/Condition/Always True")]
public class AlwaysTrueConditionSO : EnemyConditionSO
{
    public override bool IsMet(EnemyController controller) => true;
}
