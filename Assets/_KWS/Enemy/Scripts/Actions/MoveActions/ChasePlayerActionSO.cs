using UnityEngine;

/*
 * 최소 거리 ~ 최대 거리 사이의 플레이어를 추적한다. 
 */
[CreateAssetMenu(
    fileName = "ChasePlayerAction", 
    menuName = "Enemy/Action/Move/Chase Player"
)]
public class ChasePlayerActionSO : EnemyActionSO
{


    [Header("Chase Distance")]
    public float minDistance = 0.0f;    // 0이면 계속해서 추적
    public float maxDistance = 9999.0f; // 추적하기에 충분히 큰 값 -> 무조건 추적

    public override void Act(EnemyController controller)
    {
        if (controller.Player == null) return;

        float dist = Vector3.Distance(controller.transform.position, controller.Player.position);

        if (dist > minDistance && dist < maxDistance)
        {
            Vector3 dir = (controller.Player.position - controller.transform.position).normalized;
            controller.transform.Translate(dir * controller.Status.moveSpeed * Time.deltaTime);
        }
    }
}
