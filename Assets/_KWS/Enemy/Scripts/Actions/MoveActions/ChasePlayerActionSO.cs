using UnityEngine;
using UnityEngine.Localization.SmartFormat.Core.Parsing;

/*
 * 최소 거리 ~ 최대 거리 사이의 플레이어를 추적한다. 
 */
namespace Game.Enemy
{
    public enum ChaseMode { Simple, Smart }
    [CreateAssetMenu(
        fileName = "ChasePlayerAction",
        menuName = "Enemy/Action/Move/Chase Player"
    )]
    public class ChasePlayerActionSO : EnemyActionSO
    {
        public ChaseMode chaseMode = ChaseMode.Simple;
        public bool inverse = false;

        [Header("Chase Distance")]
        public float minDistance = 0.0f;    // 0이면 계속해서 추적
        public float maxDistance = 9999.0f; // 추적하기에 충분히 큰 값 -> 무조건 추적

        [Header("Smart Chase")]
        public float obstacleCheckDistance = 1.0f;

        public Vector2 enemySize = new Vector2(1f, 1f);

        public override void Act(EnemyController controller)
        {
            controller.FSM.isSuperArmor = false;

            if (controller.Player == null) return;

            float dist = Vector3.Distance(controller.transform.position, controller.Player.position);

            if (dist > minDistance && dist < maxDistance)
            {
                switch (chaseMode)
                {
                    case ChaseMode.Simple:
                        SimpleChase(controller);
                        break;
                    case ChaseMode.Smart:
                        SmartChase(controller);
                        break;
                }
            }
            else
            {
                controller.StopMove();
            }
        }

        public override void OnExit(EnemyController controller)
        {
            controller.StopMove();

            // Smart Chase 관련 변수 초기화
            controller.FSM.isChasing = false;
            controller.FSM.isBypassing = false;
            controller.FSM.bypassDirection = Vector2.zero;
        }


        private void SimpleChase(EnemyController controller)
        {

            Vector3 dir = (controller.Player.position - controller.transform.position).normalized;
            controller.MoveTo(dir, Time.fixedDeltaTime, "SimpleChase", inverse);
        }

        private void SmartChase(EnemyController controller)
        {
            if (!controller.FSM.isChasing)
            {
                // 상태 파라미터 값 설정
                controller.FSM.isChasing = true;
                controller.FSM.isBypassing = false;

                // 플레이어 방향 계산
                Vector2 toPlayer = (Vector2)(controller.Player.position - controller.transform.position).normalized;
                Vector2 boxSize = enemySize;

                RaycastHit2D hit = Physics2D.Raycast(
                    controller.transform.position,
                    toPlayer,
                    obstacleCheckDistance,
                    LayerMask.GetMask("Obstacle", "Wall")
                );

                bool inSight = !hit;

                // 디버그 참고용 -> 플레이어 방향 빨간색 라인
                Debug.DrawRay(controller.transform.position, toPlayer * obstacleCheckDistance, Color.red, 0.1f);

                if (inSight) // 플레이어가 보이는 경우 -> 바로 추적
                {
                    //Debug.LogError("추적 시도");
                    controller.MoveTo(toPlayer, Time.deltaTime, "SmartChase", inverse);
                    controller.FSM.isBypassing = false;
                }
                else // 플레이어가 보이지 않는 경우 -> 우회
                {
                    if (!controller.FSM.isBypassing && hit)
                    {
                        Vector2 wallNormal = hit.normal;

                        Vector2 perpRight = new Vector2(wallNormal.y, -wallNormal.x);
                        Vector2 perpLeft = new Vector2(-wallNormal.y, wallNormal.x);

                        float dotRight = Vector2.Dot(toPlayer, perpRight);
                        float dotLeft = Vector2.Dot(toPlayer, perpLeft);

                        Vector2 chosenDir = (dotRight > dotLeft) ? perpRight : perpLeft;

                        controller.FSM.bypassDirection = chosenDir;
                        controller.FSM.isBypassing = true;
                    }
                    
                    // 우회 방향에 장애물 여부 검사
                    bool bypassBlocked = false;
                    if (hit)
                    {
                        bypassBlocked = Physics2D.Raycast(
                            controller.transform.position,
                            controller.FSM.bypassDirection,
                            obstacleCheckDistance,
                            LayerMask.GetMask("Obstacle", "Wall")
                        );
                        Debug.DrawRay(controller.transform.position, controller.FSM.bypassDirection * obstacleCheckDistance, Color.green, 0.1f);
                    }

                    if (!bypassBlocked)
                    {
                        //Debug.LogError("우회 시도중");
                        controller.MoveTo(controller.FSM.bypassDirection, Time.deltaTime, "SmartChase", inverse);
                    }
                    else
                    {
                        //Debug.LogError("다시 우회 시도중");

                        bool bypassBlocked2 = false;
                        bypassBlocked2 = Physics2D.Raycast(
                            controller.transform.position,
                            -controller.FSM.bypassDirection,
                            obstacleCheckDistance,
                            LayerMask.GetMask("Obstacle", "Wall")
                        );
                        Debug.DrawRay(controller.transform.position, controller.FSM.bypassDirection * obstacleCheckDistance, Color.yellow, 0.1f);
                        
                        
                        if (!bypassBlocked2)
                        {
                            //Debug.LogError("또다시 우회 시도중");
                            controller.MoveTo(-controller.FSM.bypassDirection, Time.deltaTime, "SmartChase", inverse);
                        }
                        else
                        {
                            if (hit)
                            {
                                controller.transform.position += Vector3.up * 0.1f;
                            }
                        }
                    }
                }
            }
        }
    }
}
