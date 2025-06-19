using UnityEngine;


/*
 * 쿨다운 동작.
 * 일정 시간 아무 동작 없이 대기
 */
namespace Game.Enemy
{
    [CreateAssetMenu(
    fileName = "PostAttackAction",
    menuName = "Enemy/Action/State/Post Attack"
)]
    public class PostAttackActionSO : EnemyActionSO
    {
        public override void Act(EnemyController controller)
        {
            controller.FSM.isSuperArmor = false;
            // 실제 동작 없음. 필요하다면 이팩트, 방어력 증가 등 추가.
        }
    }
}
