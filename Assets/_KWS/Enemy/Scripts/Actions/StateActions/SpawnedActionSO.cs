using System.Linq;
using UnityEngine;

/*
 * 몬스터가 소환됐을 때
 */
namespace Game.Enemy
{
    [CreateAssetMenu(
    fileName = "SpawnedAction",
    menuName = "Enemy/Action/State/Spawned"
)]
    public class SpawnedActionSO : EnemyActionSO
    {
        public bool IsHideOnSpawned = true;
        public float spawnAttackCooldown = 0f;

        public override void Act(EnemyController controller)
        {
            controller.FSM.isSuperArmor = true;
            // 스폰 애니메이션, 이팩트, 사운드 등 재생

            if (!controller.FSM.isSpawnEffect)
            {
                controller.FSM.isSpawnEffect = true;
                if (IsHideOnSpawned)
                {
                    controller.SetAllChildrenActive(false);
                }
                else
                {
                    controller.SetAllChildrenActive(true);
                }
                controller.MakeSpawnEffect();

                foreach (var key in controller.lastAttackTimes.Keys.ToList())
                {
                    float cooldown = spawnAttackCooldown == 0 ? -Mathf.Infinity : Time.time + spawnAttackCooldown;
                    controller.lastAttackTimes[key] = cooldown;
                }
            }
        }

        public override void OnExit(EnemyController controller)
        {
            if (IsHideOnSpawned) controller.SetAllChildrenActive(true);
        }
    }
}
