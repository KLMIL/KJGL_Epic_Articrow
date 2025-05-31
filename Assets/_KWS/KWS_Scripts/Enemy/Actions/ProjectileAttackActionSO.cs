using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileAtackAction", menuName = "Enemy/Action/Projectile Attack")]
public class ProjectileAttackActionSO : EnemyActionSO
{
    public GameObject projectilePrefab;
    public float projectileSpeed = 8f;
    public Vector3 firePointOffset = Vector3.zero;

    public override void Act(EnemyController controller)
    {
        if (controller.Player == null || projectilePrefab == null) return;

        // 발사 위치 계산
        Vector3 firePos = controller.transform.position + firePointOffset;
        GameObject proj = Instantiate(projectilePrefab, firePos, Quaternion.identity);
        Vector3 dir = (controller.Player.position - firePos).normalized;

        // RB 또는 Transform으로 속도 부여
        // 추후, projectile Prefab 생성에 따라 변경요망 
        var rb = proj.GetComponent<Rigidbody>();
        if (rb != null) rb.linearVelocity = dir * projectileSpeed;
        controller.Animation.Play("ProjectileAttack");
    }
}
