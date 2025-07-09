using System.Collections.Generic;
using UnityEngine;

/*
 * 상, 하, 좌, 우 랜덤으로 이동. 벽에 닿으면 다른 방향 선택
 */
namespace Game.Enemy
{
    [CreateAssetMenu(
    fileName = "RandomMoveAction",
    menuName = "Enemy/Action/Move/Random Move"
)]
    public class RandomMoveActionSO : EnemyActionSO
    {
        public float minMoveCooldown = 1f;
        public float maxMoveCooldown = 2f;
        public bool inverse = false;
        public float wallCheckDistance = 0.5f;

        public override void Act(EnemyController controller)
        {
            controller.FSM.isSuperArmor = false;

            if (!controller.FSM.isRandomMoving)
            {
                controller.FSM.isRandomMoving = true;


                Vector2[] dirs = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };
                List<Vector2> possibleDirs = new List<Vector2>();

                foreach (var dir in dirs)
                {
                    RaycastHit2D hit = Physics2D.Raycast(
                            controller.transform.position,
                            dir,
                            wallCheckDistance,
                            LayerMask.GetMask("Obstacle")
                    );

                    if (hit)
                    {
                        //if (hit.distance < 0.1f)
                        //{
                        //    //Debug.LogError("벽과 너무 근접함");
                        //    Vector2 offset = hit.normal * 0.2f; // 0.1f = 보정 거리
                        //    controller.transform.position += (Vector3)offset;

                        //    continue;
                        //}
                        //continue;
                    }
                    else
                    {
                        possibleDirs.Add(dir);
                    }
                }

                if (possibleDirs.Count > 0)
                {
                    controller.FSM.randomMoveDirection = possibleDirs[Random.Range(0, possibleDirs.Count)];
                }
                else
                {
                    controller.FSM.randomMoveDirection = Vector2.zero; // Idle
                }
                    

                controller.FSM.randomMoveChangeCooldown = Random.Range(minMoveCooldown, maxMoveCooldown);
                controller.MoveTo(controller.FSM.randomMoveDirection, controller.FSM.randomMoveChangeCooldown, "Normal", inverse);
            }

            // 지속시간 갱신
            controller.FSM.randomMoveChangeCooldown -= Time.deltaTime;


            // 지속시간 완료시 다음 상태로 강제전이
            if (controller.FSM.randomMoveChangeCooldown <= 0f)
            {
                controller.FSM.ForceToNextState();
            }
        }

        public override void OnExit(EnemyController controller)
        {
            controller.StopMove();
            controller.FSM.isRandomMoving = false;
        }
    }
}
