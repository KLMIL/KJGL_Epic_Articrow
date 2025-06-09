using UnityEngine;

/*
 * 상, 하, 좌, 우 랜덤으로 이동. 벽에 닿으면 다른 방향 선택
 */
[CreateAssetMenu(
    fileName = "RandomMoveAction", 
    menuName = "Enemy/Action/Move/Random Move"
)]
public class RandomMoveActionSO : EnemyActionSO
{
    public float minMoveCooldown = 1f;
    public float maxMoveCooldown = 3f;
    //public float wallCheckDistance = 0.5f;

    public override void Act(EnemyController controller)
    {
        // 쿨타임이 지났다면 새로운 방향과 지속시간 생성
        if (controller.randomMoveChangeCooldown <= 0f)
        {
            controller.randomMoveDirection = new Vector3(
                    Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f
                ).normalized;
            controller.randomMoveChangeCooldown = Random.Range(minMoveCooldown, maxMoveCooldown);

            controller.MoveTo(controller.randomMoveDirection, controller.randomMoveChangeCooldown, "Normal");
        }

        //// 이동
        //if (!Physics.Raycast(controller.transform.position, controller.randomMoveDirection, wallCheckDistance))
        //{
        //    controller.transform.Translate(
        //        controller.randomMoveDirection * controller.Status.moveSpeed * Time.deltaTime
        //    );  
        //}
        //else
        //{
        //    // 벽에 막혀 있으면 즉시 새 방향 선택
        //    controller.randomMoveChangeCooldown = 0f;
        //}


        // 지속시간 갱신
        controller.randomMoveChangeCooldown -= Time.deltaTime;
    }

    public override void OnExit(EnemyController controller)
    {
        controller.StopMove();
    }
}
