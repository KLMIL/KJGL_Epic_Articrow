using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileAttackCooldownAction", menuName = "Enemy/Action/Projectile Attack Cooldown")]
public class ProjectileAttackCooldownSO : EnemyActionSO
{
    public override void Act(EnemyController controller)
    {
        controller.projectileFiredCount = 0;
        controller.isSpawnedMite = false; // 임시로 여기에다 작성하는데, 추후 제대로 구조 만들어야함.
        // 딱히 뭐 안할거긴 해. 추후작성. 
    }
}
