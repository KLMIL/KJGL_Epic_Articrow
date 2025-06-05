using UnityEngine;

[CreateAssetMenu(fileName = "ReceivedDamageAction", menuName = "Enemy/Action/State/Received Damage")]
public class ReceivedDamageActionSO : EnemyActionSO
{
    public float hurtDuration = 0.2f;

    public override void Act(EnemyController controller)
    {
        // 상태이상, 넉백, 이펙트 등 추가

        // ConditionSO의 트리거 Reset (경직 상태 해제 등)
        controller.isDamaged = false;
    }
}
