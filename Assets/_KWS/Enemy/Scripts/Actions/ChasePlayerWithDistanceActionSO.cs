using UnityEngine;

[CreateAssetMenu(fileName = "ChasePlayerWithDistance", menuName = "Enemy/Action/Chase Player With Distance")]
public class ChasePlayerWithDistanceActionSO : EnemyActionSO
{
    public float minDistance = 2.0f;
    public float maxDistance = 6.0f;
    public float moveSpeed = 1.5f;

    public override void Act(EnemyController controller)
    {
        // 이런 불필요한 nullcheck는 지양해야 하지 않나? 고민중
        if (controller.Player == null) return;

        float dist = Vector3.Distance(controller.transform.position, controller.Player.position);

        if (dist > minDistance && dist < maxDistance)
        {
            // 적당한 거리일 때 플레이어 쪽으로 천천히 이동
            Vector3 dir = (controller.Player.position - controller.transform.position).normalized;
            controller.transform.Translate(dir * moveSpeed * Time.deltaTime);
        }
    }
}
