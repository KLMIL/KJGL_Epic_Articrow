using UnityEngine;

[CreateAssetMenu(fileName = "PlayerInSightCondition", menuName = "Enemy/Condition/PlayerInSight")]
public class PlayerInSightConditionSO : EnemyConditionSO
{
    public float DetectionRange = 5f;

    public override bool IsMet(EnemyController controller)
    {
        if (controller.Player == null) return false;

        return Vector3.Distance(controller.transform.position, controller.Player.position) <= DetectionRange;
    }
}
