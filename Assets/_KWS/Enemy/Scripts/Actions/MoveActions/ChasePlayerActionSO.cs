using UnityEngine;

/*
 * 최소 거리 ~ 최대 거리 사이의 플레이어를 추적한다. 
 */
public enum ChaseMode { Simple, Smart }
[CreateAssetMenu(
    fileName = "ChasePlayerAction", 
    menuName = "Enemy/Action/Move/Chase Player"
)]
public class ChasePlayerActionSO : EnemyActionSO
{
    public ChaseMode chaseMode = ChaseMode.Simple;

    [Header("Chase Distance")]
    public float minDistance = 0.0f;    // 0이면 계속해서 추적
    public float maxDistance = 9999.0f; // 추적하기에 충분히 큰 값 -> 무조건 추적

    [Header("Smart Chase")]
    public float obstacleCheckDistance = 0.2f;

    public override void Act(EnemyController controller)
    {
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
    }


    private void SimpleChase(EnemyController controller)
    {
        Vector3 dir = (controller.Player.position - controller.transform.position).normalized;
        controller.MoveTo(dir, Time.deltaTime, "SimpleChase");

        //controller.transform.Translate(dir * controller.Status.moveSpeed * Time.deltaTime);
    }

    private void SmartChase(EnemyController controller)
    {
        Vector3 dir = (controller.Player.position - controller.transform.position).normalized;

        Vector3 chosenDir = dir;

        if (Physics2D.Raycast(controller.transform.position, dir, obstacleCheckDistance))
        {
            // 오른쪽 시도
            Vector3 right = Vector3.Cross(Vector3.forward, dir).normalized;
            if (!Physics2D.Raycast(controller.transform.position, right, obstacleCheckDistance))
            {
                //controller.transform.Translate(right * controller.Status.moveSpeed * Time.deltaTime);
                chosenDir = right;
                //return;
            }
            else
            {
                // 왼쪽 시도
                Vector3 left = -right;
                if (!Physics2D.Raycast(controller.transform.position, left, obstacleCheckDistance))
                {
                    //controller.transform.Translate(left * controller.Status.moveSpeed * Time.deltaTime);
                    chosenDir = left;
                    //return;
                }
                else
                {
                    // 둘다 막히면 움직이지 않음
                    chosenDir = Vector3.zero;
                }
            }

            // 둘다 막히면 움직이지 않음
        }
        else
        {
            //controller.transform.Translate(dir * controller.Status.moveSpeed * Time.deltaTime);
        }

        if (chosenDir != Vector3.zero)
        {
            controller.MoveTo(chosenDir, Time.deltaTime, "SmartChase");
        }
        else
        {
            controller.StopMove();
        }
    }
}
