using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileAttackCooldownAction", menuName = "Enemy/Action/Projectile Attack Cooldown")]
public class ProjectileAttackCooldownSO : EnemyActionSO
{
    public override void Act(EnemyController controller)
    {
        controller.projectileFiredCount = 0;
        // 딱히 뭐 안할거긴 해. 추후작성. 
    }
}
