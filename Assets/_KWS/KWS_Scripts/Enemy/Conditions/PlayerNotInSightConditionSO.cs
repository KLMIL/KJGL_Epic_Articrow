using UnityEngine;

[CreateAssetMenu(fileName = "PlayerNotInSightCondition", menuName = "Enemy/Condition/PlayerNotInSight")]
public class PlayerNotInSightConditionSO : EnemyConditionSO
{
    public float DetectionRange = 5f;

    public override bool IsMet(EnemyController controller)
    {
        if (controller.Player == null) return true;

        return Vector3.Distance(controller.transform.position, controller.Player.position) > DetectionRange;
    }
}
