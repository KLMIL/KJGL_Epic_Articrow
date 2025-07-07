using UnityEngine;

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
        public float obstacleCheckDistance = 1f;

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

                //bool inSight = !Physics2D.BoxCast(
                //        (Vector2)controller.transform.position,
                //        boxSize,
                //        0f,
                //        toPlayer,
                //        obstacleCheckDistance,
                //        LayerMask.GetMask("Obstacle")
                //    );

                bool inSight = !Physics2D.Raycast(controller.transform.position, toPlayer, obstacleCheckDistance, LayerMask.GetMask("Obstacle"));
                //Debug.DrawRay(controller.transform.position, controller.FSM.bypassDirection * obstacleCheckDistance, Color.green, 0.1f);
                //Debug.DrawRay(controller.transform.position, toPlayer * obstacleCheckDistance, Color.red, 0.1f);

                // 디버그 참고용 -> 박스로 그리진 않음
                Debug.DrawRay(controller.transform.position, toPlayer * obstacleCheckDistance, Color.red, 0.1f);

                if (inSight) // 플레이어가 보이는 경우 -> 바로 추적
                {
                    Debug.LogError("추적 시도");
                    controller.MoveTo(toPlayer, Time.deltaTime, "SmartChase", inverse);
                    controller.FSM.isBypassing = false;
                }
                else // 플레이어가 보이지 않는 경우 -> 우회
                {
                    if (!controller.FSM.isBypassing)
                    {
                        // 좌/우 방향 결정
                        Vector2 right = new Vector2(-toPlayer.y, toPlayer.x);
                        Vector2 left = -right;

                        Vector2[] candidates = { right, left };
                        controller.FSM.bypassDirection = candidates[Random.Range(0, 2)];
                        controller.FSM.isBypassing = true;
                    }

                    // 또 막히면 다른 방향 시도
                    //bool bypassBlocked = Physics2D.BoxCast(
                    //        (Vector2)controller.transform.position,
                    //        boxSize,
                    //        0f,
                    //        controller.FSM.bypassDirection,
                    //        obstacleCheckDistance,
                    //        LayerMask.GetMask("Obstacle")
                    //    );

                    bool bypassBlocked = Physics2D.Raycast(controller.transform.position, controller.FSM.bypassDirection, obstacleCheckDistance, LayerMask.GetMask("Obstacle"));
                    //Debug.DrawRay(controller.transform.position, controller.FSM.bypassDirection * obstacleCheckDistance, Color.green, 0.1f);
                    //Debug.DrawRay(controller.transform.position, toPlayer * obstacleCheckDistance, Color.red, 0.1f);

                    // 디버그 참고용 -> 박스로 그리진 않음
                    Debug.DrawRay(controller.transform.position, controller.FSM.bypassDirection * obstacleCheckDistance, Color.green, 0.1f);

                    if (!bypassBlocked)
                    {
                        //Debug.LogError("우회 시도");
                        controller.MoveTo(controller.FSM.bypassDirection, Time.deltaTime, "SmartChase", inverse);
                    }
                    else
                    {
                        // 막히면 정지 + idle로 전환
                        controller.StopMove();
                        controller.FSM.ChangeState("Idle");
                    }
                }
            }
        }
    }
}
