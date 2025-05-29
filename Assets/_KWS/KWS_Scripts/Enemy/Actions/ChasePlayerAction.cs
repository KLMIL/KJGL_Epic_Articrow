using UnityEngine;

public class ChasePlayerAction : IEnemyAction
{
    public void Act(EnemyController controller)
    {
        if (controller.Player == null) return;

        Vector3 direction = (controller.Player.position - controller.transform.position).normalized;
        controller.transform.Translate(direction * controller.Status.moveSpeed * Time.deltaTime);
        controller.Animation.Play("Chase");
    }
}
