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
        public override void Act(EnemyController controller)
        {
            controller.FSM.isSuperArmor = false;
            // 스폰 애니메이션, 이팩트, 사운드 등 재생
        }
    }
}
