using UnityEngine;

[CreateAssetMenu(fileName = "RushAttackCooldownAction", menuName = "Enemy/Action/State/Rush Attack Cooldown")]
public class RushAttackCooldownActionSO : EnemyActionSO
{
    public override void Act(EnemyController controller)
    {
        if (controller.RushAttackTrigger != null)
        {
            controller.RushAttackTrigger.SetActive(false);
        }
    }
}
