using UnityEngine;

[CreateAssetMenu(fileName = "ChasePlayerAction", menuName = "Enemy/Action/Chase Player")]
public class ChasePlayerActionSO : EnemyActionSO
{
    public override void Act(EnemyController controller)
    {
        if (controller.Player == null) return;

        Vector3 direction = (controller.Player.position - controller.transform.position).normalized;
        controller.transform.Translate(direction * controller.Status.moveSpeed * Time.deltaTime);
        //controller.Animation.Play("Chase");
    }
}
