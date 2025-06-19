using UnityEngine;

namespace Game.Enemy
{
    [CreateAssetMenu(fileName = "DamagedCondition", menuName = "Enemy/Condition/Damaged")]
    public class DamagedConditionSO : EnemyConditionSO
    {
        public override bool IsMet(EnemyController controller)
        {
            return controller.FSM.isDamaged && !controller.FSM.isSuperArmor;
        }
    }
}
