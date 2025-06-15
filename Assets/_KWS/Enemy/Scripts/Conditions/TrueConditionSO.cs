using UnityEngine;

namespace Game.Enemy
{
    [CreateAssetMenu(fileName = "TrueCondition", menuName = "Enemy/Condition/True")]
    public class TrueConditionSO : EnemyConditionSO
    {
        public override bool IsMet(EnemyController controller) => true;
    }
}
