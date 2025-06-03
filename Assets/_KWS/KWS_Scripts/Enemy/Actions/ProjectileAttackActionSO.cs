using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileAtackAction", menuName = "Enemy/Action/Projectile Attack")]
public class ProjectileAttackActionSO : EnemyActionSO
{
    public GameObject projectilePrefab;
    public float projectileSpeed = 8f;
    public Vector3 firePointOffset = Vector3.zero;
    public int projectileAmount = 3;
    public float projectileTurm = 0.05f;


    public override void Act(EnemyController controller)
    {
        // null 할당 오류 return
        if (controller.Player == null || projectilePrefab == null) return;

        if (controller.projectileFiredCount >= projectileAmount) return;

        controller.projectileIntervalTimer += Time.deltaTime;
        if (controller.projectileIntervalTimer >= projectileTurm)
        {
            controller.projectileIntervalTimer = 0f;
            controller.projectileFiredCount++;

            Vector3 firePos = controller.transform.position + firePointOffset;
            Vector3 dir = (controller.Player.position - firePos).normalized;

            GameObject currProj = Instantiate(projectilePrefab, firePos, Quaternion.identity, controller.transform);
            Destroy(currProj, 1f);

            var rb = currProj.GetComponent<Rigidbody2D>();
            if (rb != null) rb.linearVelocity = dir * projectileSpeed;
        }
    }
}
