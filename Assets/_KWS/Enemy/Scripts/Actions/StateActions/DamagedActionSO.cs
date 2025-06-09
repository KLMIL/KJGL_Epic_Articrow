using UnityEngine;

[CreateAssetMenu(
    fileName = "DamagedAction", 
    menuName = "Enemy/Action/State/Damaged"
)]
public class DamagedActionSO : EnemyActionSO
{
    public float hurtDuration = 0.2f;

    public override void Act(EnemyController controller)
    {
        controller.Status.healthPoint -= controller.FSM.pendingDamage;
        controller.FSM.pendingDamage = 0;
        controller.FSM.isDamaged = false;

        // (상태이상, 넉백, 이펙트 등 추가)
    }
}
