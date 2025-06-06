using UnityEngine;

[CreateAssetMenu(fileName = "PlayerNotInDistanceCondition", menuName = "Enemy/Condition/Player Not In Distance")]
public class PlayerNotInDistanceConditionSO : EnemyConditionSO
{
    public float distance = 5f;

    public override bool IsMet(EnemyController controller)
    {
        if (controller.Player == null) return true;

        return !(Vector3.Distance(controller.transform.position, controller.Player.position) <= distance);
    }
}
