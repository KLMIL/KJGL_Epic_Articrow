using UnityEngine;

[CreateAssetMenu(fileName = "AlwaysTureCondition", menuName = "Enemy/Condition/AlwaysTrue")]
public class AlwaysTrueConditionSO : EnemyConditionSO
{
    public override bool IsMet(EnemyController controller) => true;
}
