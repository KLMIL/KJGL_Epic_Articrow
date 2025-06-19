using UnityEngine;

/*
 * 몬스터의 Idle(정지/대기) 상태. 아무것도 하지 않음.
 */
namespace Game.Enemy
{
    [CreateAssetMenu(
    fileName = "IdleAction",
    menuName = "Enemy/Action/State/Idle"
)]
    public class IdleActionSO : EnemyActionSO
    {
        public override void Act(EnemyController controller)
        {
            controller.FSM.isSuperArmor = false;
            // 아무것도 하지 않음.
        }
    }
}
