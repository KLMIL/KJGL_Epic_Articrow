using UnityEngine;

namespace Game.Enemy
{
    [CreateAssetMenu(fileName = "FalseCondition", menuName = "Enemy/Condition/False")]
    public class FalseConditionSO : EnemyConditionSO
    {
        public override bool IsMet(EnemyController controller) => false;
    }
}
