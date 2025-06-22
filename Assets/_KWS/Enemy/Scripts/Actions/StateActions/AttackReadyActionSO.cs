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

        public override void OnEnter(EnemyController controller)
        {
            controller._attackIndicator?.Show();
        }

        public override void Act(EnemyController controller)
        {
            controller.FSM.isSuperArmor = true;

            if (controller._attackIndicator != null)
            {
                Vector2 dir = (controller.Player.position - controller.transform.position).normalized;
                float len = (controller.Player.position - controller.transform.position).magnitude * 2;

                len = len < controller.FSM.indicatorLength * 2 ? len : controller.FSM.indicatorLength * 2;

                controller._attackIndicator?.SetDirection(dir, len);
            }
        }

        public override void OnExit(EnemyController controller)
        {
            controller._attackIndicator?.Hide();
        }
    }
}
