using UnityEngine;

/*
 * 적과 플레이어가 닿았을 때 검출
 */
[CreateAssetMenu(
    fileName = "ContactToPlayerCondition",
    menuName = "Enemy/Condition/Contact To Player"
)]
public class ContactToPlayerConditionSO: EnemyConditionSO
{
    public override bool IsMet(EnemyController controller)
    {
        return controller.FSM.isContactDamageActive;
    }
}