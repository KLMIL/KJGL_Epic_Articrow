using UnityEngine;

/*
 * 플레이어에게서 멀어지는 방향으로 이동한다. 
 * Simple Move = 그냥 뒤로 갈 수 있는만큼 간다
 * Smart Move = 뒤로 갈 수 없다면, 거리까지 가기 위해 다른 길을 탐색한다. 
 */
public enum MoveAwayMode { Simple, Smart }

[CreateAssetMenu(
    fileName = "MoveAwayAction", 
    menuName = "Enemy/Action/Move/Move Away"
)]
public class MoveAwayActionSO : EnemyActionSO
{
    public float moveDistance = 3.0f;
    public MoveAwayMode mode = MoveAwayMode.Simple;
    public float wallCheckDistance = 0.5f;

    public override void Act(EnemyController controller)
    {
        if (controller.Player == null) return;

        Vector3 awayDir = (controller.transform.position - controller.Player.position).normalized;
        Vector3 targetPos = controller.transform.position + awayDir * moveDistance;


        switch (mode)
        {
            case MoveAwayMode.Simple:
                if (!Physics.Raycast(controller.transform.position, awayDir, wallCheckDistance))
                {
                    controller.transform.Translate(awayDir * controller.Status.moveSpeed * Time.deltaTime);
                }
                break;

            case MoveAwayMode.Smart:
                // 벽/장애물 체크
                if (Physics.Raycast(controller.transform.position, awayDir, out RaycastHit hit, wallCheckDistance))
                {
                    // 벽에 막혀있으면, 벽을 따라 '오른쪽' or '왼쪽' 방향으로 이동
                    Vector3 right = Vector3.Cross(Vector3.up, awayDir).normalized;
                    Vector3 left = -right;
                    // 오른쪽 방향 우선 체크
                    if (!Physics.Raycast(controller.transform.position, right, wallCheckDistance))
                    {
                        controller.transform.Translate(right * controller.Status.moveSpeed * Time.deltaTime);
                    }
                    // 왼쪽 방향 우선 체크
                    else if (!Physics.Raycast(controller.transform.position, left, wallCheckDistance))
                    {
                        controller.transform.Translate(left * controller.Status.moveSpeed * Time.deltaTime);
                    }
                    // 둘 다 막혀있으면 멈춤
                }
                else
                {
                    controller.transform.Translate(awayDir * controller.Status.moveSpeed * Time.deltaTime);
                }
                break;
        }
    }
}
