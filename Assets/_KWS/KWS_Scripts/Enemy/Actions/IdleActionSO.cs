using UnityEngine;

[CreateAssetMenu(fileName = "IdleAction", menuName = "Enemy/Action/Idle")]
public class IdleActionSO : EnemyActionSO
{
    public override void Act(EnemyController controller)
    {
        controller.Animation.Play("Idle");
    }
}
