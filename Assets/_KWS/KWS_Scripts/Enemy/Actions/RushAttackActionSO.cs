using UnityEngine;

[CreateAssetMenu(fileName = "RushAttackAction", menuName = "Enemy/Action/Rush Attack")]
public class RushAttackActionSO : EnemyActionSO
{
    public float rushSpeed = 6f;
    public float attackDuration = 1f;
    float elapsed = 0f;
    Vector3 _rushDirection;
    bool startRush = false;


    public override void Act(EnemyController controller)
    {
        if (controller.Player == null) return;

        if (!startRush)
        {
            _rushDirection = (controller.Player.position - controller.transform.position).normalized;
            // 돌진 공격 피격 Collider 활성화. RushAttackCooldownAction에서 비활성화 처리.
            if (controller.RushAttackTrigger != null)
            {
                controller.RushAttackTrigger.SetActive(true);
            }

            startRush = true;
        }

        if (elapsed < attackDuration)
        {
            controller.transform.Translate(_rushDirection * rushSpeed * Time.deltaTime);
            //controller.Animation.Play("RushAttack");
            elapsed += Time.deltaTime;
        }
        else
        {
            elapsed = 0f;
            startRush = false;
        }
    }
}
