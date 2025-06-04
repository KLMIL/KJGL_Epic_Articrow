using UnityEngine;

[CreateAssetMenu(fileName = "PlayerInDangerDistanceCondition", menuName = "Enemy/Condition/Player In Danger Distance")]
public class PlayerInDangerDistanceSO : EnemyConditionSO
{
    public float dangerDistance = 2.0f;

    public override bool IsMet(EnemyController controller)
    {
        if (controller.Player == null) return false;

        return Vector3.Distance(controller.transform.position, controller.Player.position) <= dangerDistance;
    }
}
