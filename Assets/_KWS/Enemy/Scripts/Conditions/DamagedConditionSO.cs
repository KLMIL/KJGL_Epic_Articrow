using UnityEngine;

[CreateAssetMenu(fileName = "DamagedCondition", menuName = "Enemy/Condition/Damaged")]
public class DamagedConditionSO : EnemyConditionSO
{
    public override bool IsMet(EnemyController controller)
    {
        return controller.isDamaged;
    }
}
