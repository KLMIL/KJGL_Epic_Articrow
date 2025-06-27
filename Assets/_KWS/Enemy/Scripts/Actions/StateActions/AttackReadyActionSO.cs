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
            controller.FSM.isSuperArmor = true;
            controller.FSM.AttackTargetPosition = controller.Player.position;

            if (controller._attackIndicator != null)
            {
                Vector2 dir = (controller.FSM.AttackTargetPosition - (Vector2)controller.transform.position).normalized;
                float len = (controller.Player.position - controller.transform.position).magnitude * 2;

                len = len < controller.FSM.indicatorLength * 2 ? len : controller.FSM.indicatorLength * 2;

                //controller._attackIndicator?.SetDirection(dir, len);
                // 임시: 항상 최대 공격 사거리로 indicator 생성
                controller._attackIndicator?.SetDirection(dir, controller.FSM.indicatorLength * 2);
            }

            controller._attackIndicator?.Show();
        }

        public override void Act(EnemyController controller)
        {
            // 효율을 위해 아무것도 하지 않음. 
        }

        public override void OnExit(EnemyController controller)
        {
            controller._attackIndicator?.BlinkAndHide();
            //controller._attackIndicator?.Hide();
        }
    }
}
