using UnityEngine;

[CreateAssetMenu(fileName = "PlayerInRangeCondition", menuName = "Enemy/Condition/PlayerInRnage")]
public class PlayerInRangeConditionSO : EnemyConditionSO
{
    public float triggerRange = 2.0f;

    public override bool IsMet(EnemyController controller)
    {
        if (controller.Player == null) return false;

        return Vector3.Distance(controller.transform.position, controller.Player.position) <= triggerRange;
    }
}
