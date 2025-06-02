using UnityEngine;

[CreateAssetMenu(fileName = "DeadCondition", menuName = "Enemy/Condition/Dead")]
public class DeadConditionSO : EnemyConditionSO
{
    public override bool IsMet(EnemyController controller)
    {
        return controller.Status.healthPoint <= 0f;
    }
}
