using UnityEngine;

/*
 * 투사체 공격
 * Normal: 일반 투사체 발사 공격
 */

public enum ProjectileAttackMode { Normal }
[CreateAssetMenu(
    fileName = "ProjectileAtackAction", 
    menuName = "Enemy/Action/Attack/Projectile Attack"
)]
public class ProjectileAttackActionSO : EnemyActionSO
{
    public ProjectileAttackMode projectileAttackMode = ProjectileAttackMode.Normal;

    public Vector3 firePointOffset = Vector3.zero;
    
    public GameObject projectilePrefab;
    public float projectileSpeed = 8f;
    public int projectileAmount = 3;
    public float projectileTurm = 0.05f;

    public float damageMultiply = 1.0f;
    public float cooldown = 1.0f;


    public override void Act(EnemyController controller)
    {
        //// 현재 공격 상태의 쿨타임 체크
        //string key = controller.CurrentStateName;
        //if (!controller.lastAttackTimes.ContainsKey(key))
        //{
        //    controller.lastAttackTimes[key] = -Mathf.Infinity;
        //}

        //if (Time.time - controller.lastAttackTimes[key] < cooldown) return;


        switch (projectileAttackMode)
        {
            case ProjectileAttackMode.Normal:
                NormalAttack(controller);
                break;
        }

        //// 쿨타임 부여
        //controller.lastAttackTimes[key] = Time.time;
    }

    public override void OnEnter(EnemyController controller)
    {
        controller.lastAttackTimes[controller.CurrentStateName] = -Mathf.Infinity;
    }

    public override void OnExit(EnemyController controller)
    {
        controller.projectileFiredCount = 0;
        controller.projectileIntervalTimer = 0;
    }

    private void NormalAttack(EnemyController controller)
    {
        // null 할당 오류 return
        if (controller.Player == null || projectilePrefab == null) return;
        if (controller.projectileFiredCount >= projectileAmount) return;

        controller.projectileIntervalTimer += Time.deltaTime;
        while (controller.projectileIntervalTimer >= projectileTurm)
        {
            controller.projectileIntervalTimer = 0f;
            controller.projectileFiredCount++;

            Vector3 firePos = controller.transform.position + firePointOffset;
            Vector3 dir = (controller.Player.position - firePos).normalized;

            GameObject currProj = Instantiate(projectilePrefab, firePos, Quaternion.identity, controller.transform);
            Destroy(currProj, 1f);

            var rb = currProj.GetComponent<Rigidbody2D>();
            if (rb != null) rb.linearVelocity = dir * projectileSpeed;

            if (controller.projectileFiredCount >= projectileAmount)
                break; // 모든 발사 끝내면 중단
        }
    }
}
