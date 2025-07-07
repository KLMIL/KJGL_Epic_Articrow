using UnityEngine;

/*
 * 플레이어에게서 멀어지는 방향으로 이동한다. 
 * Simple Move = 그냥 뒤로 갈 수 있는만큼 간다
 * Smart Move = 뒤로 갈 수 없다면, 거리까지 가기 위해 다른 길을 탐색한다. 
 */
namespace Game.Enemy
{
    public enum MoveAwayMode { Simple, Smart }

    [CreateAssetMenu(
        fileName = "MoveAwayAction",
        menuName = "Enemy/Action/Move/Move Away"
    )]
    public class MoveAwayActionSO : EnemyActionSO
    {
        public float moveDistance = 3.0f;
        public float moveDuration = 0.8f;
        public MoveAwayMode mode = MoveAwayMode.Simple;
        public float wallCheckDistance = 0.5f;
        public bool inverse = false;
        public bool isSuperArmor = false;

        public override void Act(EnemyController controller)
        {
            controller.FSM.isSuperArmor = isSuperArmor;

            if (controller.Player == null) return;

            if (!controller.FSM.isMovingAway)
            {
                controller.FSM.isMovingAway = true;
                controller.FSM.isBypassingMoveAway = false;
            }

            Vector3 awayDir = (controller.transform.position - controller.Player.position).normalized;
            Vector3 targetPos = controller.transform.position + awayDir * moveDistance;


            switch (mode)
            {
                case MoveAwayMode.Simple:
                    {
                        controller.MoveTo(awayDir, moveDuration, "SimpleMoveAway", inverse);
                        break;
                    }
                case MoveAwayMode.Smart:
                    {
                        // 벽/장애물 체크
                        Vector3 chosenDir = awayDir;
                        if (Physics2D.Raycast(controller.transform.position, awayDir, wallCheckDistance, LayerMask.GetMask("Obstacle")))
                        {
                            if (!controller.FSM.isBypassingMoveAway)
                            {
                                controller.FSM.isBypassingMoveAway = true;

                                Vector2 right = (Vector2)Vector3.Cross(Vector3.forward, awayDir).normalized;
                                Vector2 left = -right;

                                Vector2[] dirs = { right, left };
                                Vector2 bypass = dirs[Random.Range(0, 2)];

                                bool bypassBlocked = Physics2D.Raycast(controller.transform.position, bypass, wallCheckDistance, LayerMask.GetMask("Obstacle"));

                                if (!bypassBlocked)
                                {
                                    chosenDir = bypass;
                                }
                                else
                                {
                                    bypass = -bypass;
                                    bool otherBlocked = Physics2D.Raycast(controller.transform.position, bypass, wallCheckDistance, LayerMask.GetMask("Obstacle"));
                                    if (!otherBlocked)
                                    {
                                        chosenDir = bypass;

                                    }
                                    else
                                    {
                                        chosenDir = Vector3.zero;
                                        controller.FSM.ChangeState("Idle");
                                    }
                                }

                                controller.FSM.bypassDirection = chosenDir;
                            }
                            else
                            {
                                bool bypassBlocked = Physics2D.Raycast(controller.transform.position, controller.FSM.bypassDirection, wallCheckDistance, LayerMask.GetMask("Obstacle"));

                                if (bypassBlocked)
                                {
                                    controller.FSM.isBypassingMoveAway = false;
                                    return;
                                }
                                else
                                {
                                    chosenDir = controller.FSM.bypassDirection;
                                }
                            }
                        }

                        //Debug.LogError($"Move Dir: {chosenDir.x}, {chosenDir.y}");
                        controller.MoveTo(chosenDir, moveDuration, "SmartMoveAway", inverse);
                        break;
                    }
            }
        }

        public override void OnExit(EnemyController controller)
        {
            controller.FSM.isMovingAway = false;
            controller.StopMove();
        }
    }
}
