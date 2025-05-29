using UnityEngine;

public class PlayerInSightCondition : IEnemyCondition
{
    public float DetectionRange = 5f;

    public bool IsMet(EnemyController controller)
    {
        if (controller.Player == null) return false;

        return Vector3.Distance(controller.transform.position, controller.Player.position) <= DetectionRange;
    }
}
