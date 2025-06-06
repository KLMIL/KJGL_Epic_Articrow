using UnityEngine;

[CreateAssetMenu(fileName = "PlayerInRangeCondition", menuName = "Enemy/Condition/Player In Range")]
public class PlayerInDistanceConditionSO : EnemyConditionSO
{
    public float distance = 5f;

    public override bool IsMet(EnemyController controller)
    {
        if (controller.Player == null) return false;

        return Vector3.Distance(controller.transform.position, controller.Player.position) <= distance;
    }
}
