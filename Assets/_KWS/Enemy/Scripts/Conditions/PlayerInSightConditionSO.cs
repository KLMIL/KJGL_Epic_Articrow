using UnityEngine;

namespace Game.Enemy
{
    [CreateAssetMenu(fileName = "PlayerInSightCondition", menuName = "Enemy/Condition/Player In Sight")]
    public class PlayerInSightConditionSO : EnemyConditionSO
    {
        public float detectionRange = 5f;

        public override bool IsMet(EnemyController controller)
        {
            if (controller.Player == null) return false;

            return Vector3.Distance(controller.transform.position, controller.Player.position) <= detectionRange;
        }
    }
}
