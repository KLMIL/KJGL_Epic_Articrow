using UnityEngine;

/*
 * 플레이어와 일정 거리를 유지한다. 
 */
namespace Game.Enemy
{
    [CreateAssetMenu(
        fileName = "OrbitAction",
        menuName = "Enemy/Action/Move/Orbit"
    )]
    public class OrbitActionSO : EnemyActionSO
    {
        public float StandardDistance = 4f; // 기준 거리
        public float DistancePadding = 0.2f; // 기준 거리 오차범위
        public float MoveSpeedMultiply = 1.0f;
        public float ChangeDirectionInterval = 1f;

        public override void OnEnter(EnemyController controller)
        {
            // 속도 배율 적용
            controller.ChangeMoveSpeedMultiply(MoveSpeedMultiply);
            controller.FSM.isOrbit = true;
        }

        public override void Act(EnemyController controller)
        {
            if (Time.time - controller.FSM.orbitChagneTime < ChangeDirectionInterval) return;

            // 플레이어와의 거리 계산
            Vector2 playerDir = controller.Player.position - controller.transform.position;
            float playerDist = playerDir.magnitude;

            Vector2 moveDir = Vector2.zero;


            // 기준 거리 값 + 패딩보다 멀거나, 기준 거리 값 - 패딩보다 가깝다면 멀어지거나 가까워지는 방향으로 이동
            float minOrbit = StandardDistance - DistancePadding;
            float maxOrbit = StandardDistance + DistancePadding;
            
            if (maxOrbit < playerDist)
            {
                moveDir = playerDir.normalized;
            }
            else if (playerDist < minOrbit)
            {
                moveDir = -playerDir.normalized;
            }
            else // 해당 거리 안이라면 플레이어와 수직한 방향(좌, 우 랜덤)으로 이동
            {
                moveDir = Vector3.Cross(playerDir.normalized, Vector3.forward) * (Random.value > 0.5f ? 1 : -1);
                moveDir.Normalize();
            }

            // 가려는 방향이 막혀있다면 반전
            bool isblocked = Physics2D.Raycast(controller.transform.position, moveDir, 0.5f, LayerMask.GetMask("Obstacle"));
            if (isblocked)
            {
                moveDir *= -1;
            }

            // 이동 호출
            controller.FSM.orbitChagneTime = Time.time;
            controller.MoveTo(moveDir, ChangeDirectionInterval, "Orbit", false);
        }

        public override void OnExit(EnemyController controller)
        {
            // 속도 배율 초기화
            controller.StopMove();
            controller.ChangeMoveSpeedMultiply(1.0f);
            controller.FSM.isOrbit = false;
        }
    }
}

