using UnityEngine;

[CreateAssetMenu(fileName = "PlayerNotInRangeCondition", menuName = "Enemy/Condition/Player Not In Range")]
public class PlayerNotInDistanceConditionSO : EnemyConditionSO
{
    public float distance = 5f;

    public override bool IsMet(EnemyController controller)
    {
        if (controller.Player == null) return true;

        return !(Vector3.Distance(controller.transform.position, controller.Player.position) <= distance);
    }
}
