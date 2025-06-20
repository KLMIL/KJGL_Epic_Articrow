using UnityEngine;

/*
 * 몬스터의 Attack Ready 상태. 아무것도 하지 않음.
 */
namespace Game.Enemy
{
    [CreateAssetMenu(
    fileName = "AttackReadyAction",
    menuName = "Enemy/Action/State/AttackReady"
)]
    public class AttackReadyActionSO : EnemyActionSO
    {
        public override void Act(EnemyController controller)
        {
            controller.FSM.isSuperArmor = true;
            // 아무것도 하지 않음.
        }
    }
}
