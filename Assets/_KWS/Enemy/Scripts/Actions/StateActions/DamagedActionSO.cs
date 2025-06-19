using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

namespace Game.Enemy
{
    [CreateAssetMenu(
    fileName = "DamagedAction",
    menuName = "Enemy/Action/State/Damaged"
)]
    public class DamagedActionSO : EnemyActionSO
    {
        public float hurtDuration = 0.2f;

        public override void Act(EnemyController controller)
        {
            controller.FSM.isSuperArmor = false;

            //controller.Status.healthPoint -= controller.FSM.pendingDamage;
            //controller.FSM.pendingDamage = 0;
            controller.FSM.isDamaged = false;

            // (상태이상, 이펙트 등 추가)

            // 넉백
            Vector2 knockbackDir = (controller.transform.position - controller.Attacker.position).normalized;
            float knockbackDist = controller.FSM.knockbackDistance * (1 - controller.Status.knockbackResist);

            controller.StartKnockbackCoroutine(knockbackDir, knockbackDist, hurtDuration / 2, 4);
        }
    }
}