using UnityEngine;

[CreateAssetMenu(fileName = "MoveAwayAction", menuName = "Enemy/Action/Move Away")]
public class MoveAwayActionSO : EnemyActionSO
{
    public float moveAwaySpeed = 4f;

    public override void Act(EnemyController controller)
    {
        if (controller.Player == null) return;

        // Player와 반대 방향 벡터
        Vector3 dir = (controller.transform.position - controller.Player.position).normalized;
        controller.transform.Translate(dir * controller.Status.moveSpeed * Time.deltaTime);
        //controller.Animation.Play("MoveAway");
    }
}
