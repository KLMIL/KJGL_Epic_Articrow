using UnityEngine;

public class IdleAction : IEnemyAction
{
    public void Act(EnemyController controller)
    {
        controller.Animation.Play("Idle");
    }
}
