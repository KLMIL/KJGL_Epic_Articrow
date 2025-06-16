using System;
using System.Collections;
using UnityEngine;

/*
 * 투사체 공격
 * Normal: 일반 투사체 발사 공격
 */
namespace Game.Enemy
{
    public enum ProjectileAttackMode 
    { 
        LinearAttack,
        LinearSpawn,
        ParabolaAttack,
        ParabolaSpawn
    }
    [CreateAssetMenu(
        fileName = "ProjectileAtackAction",
        menuName = "Enemy/Action/Attack/Projectile Attack"
    )]
    public class ProjectileAttackActionSO : EnemyActionSO
    {
        public ProjectileAttackMode projectileAttackMode = ProjectileAttackMode.LinearAttack;

        public Vector3 firePointOffset = Vector3.zero;

        public GameObject projectilePrefab;
        public float projectileSpeed = 8f;
        public int projectileAmount = 3;
        public float projectileTurm = 0.05f;
        public float lifetime = 1.0f;

        public float damageMultiply = 1.0f;
        public float cooldown = 1.0f;


        public override void Act(EnemyController controller)
        {
            // 현재 공격 상태의 쿨타임 체크
            // Behaviour로 체크하고 있기 때문에 없어도 되는 부분이지만,
            // 추후 버그 발생 가능성을 줄이기 위해 유지
            string key = controller.CurrentStateName;
            if (!controller.lastAttackTimes.ContainsKey(key))
            {
                controller.lastAttackTimes[key] = -Mathf.Infinity;
            }

            if (Time.time - controller.lastAttackTimes[key] < cooldown) return;


            switch (projectileAttackMode)
            {
                case ProjectileAttackMode.LinearAttack:
                    controller.FSM.fireRoutine = controller.StartCoroutine(FireLinear(controller, false));
                    break;
                case ProjectileAttackMode.LinearSpawn:
                    controller.FSM.fireRoutine = controller.StartCoroutine(FireLinear(controller, true));
                    break;
                case ProjectileAttackMode.ParabolaAttack:
                    controller.FSM.fireRoutine = controller.StartCoroutine(FireParabola(controller, false));
                    break;
                case ProjectileAttackMode.ParabolaSpawn:
                    controller.FSM.fireRoutine = controller.StartCoroutine(FireParabola(controller, true));
                    break;
            }

            //// 쿨타임 부여
            controller.lastAttackTimes[key] = Time.time;
        }

        public override void OnExit(EnemyController controller)
        {
            if (controller.FSM.fireRoutine != null)
            {
                controller.StopCoroutine(controller.FSM.fireRoutine);
            }
            controller.FSM.projectileFiredCount = 0;
            controller.FSM.projectileIntervalTimer = 0;
        }


        private IEnumerator FireLinear(EnemyController controller, bool isSpawn)
        {
            int count = 0;

            // 같은 방향으로 발사하기 위해 방향 함수를 while문 밖으로 뺌
            Vector3 firePos = controller.transform.position + firePointOffset;
            Vector3 dir = (controller.Player.position - firePos).normalized;
            Vector2 velocity = dir * projectileSpeed;

            if (dir.x != 0)
            {
                controller.SpriteRenderer.flipX = dir.x > 0;
            }

            while (count < projectileAmount)
            {
                GameObject currProj = Instantiate(projectilePrefab, firePos, Quaternion.identity);
                EnemyProjectile proj = currProj.GetComponent<EnemyProjectile>();

                if (proj != null)
                {
                    proj.InitProjecitle(
                            controller,
                            controller.Status.attack * damageMultiply,
                            velocity,
                            0.0f,
                            isSpawn ? projectilePrefab : null,
                            isSpawn,
                            lifetime
                        );
                }
                count++;
                yield return new WaitForSeconds(projectileTurm);
            }
        }

        private IEnumerator FireParabola(EnemyController controller, bool isSpawn)
        {
            int count = 0;

            while (count < projectileAmount)
            {
                Vector2 firePos = controller.transform.position + firePointOffset;
                float angle = UnityEngine.Random.Range(30f, 150f);
                float rad = angle * Mathf.Deg2Rad;
                Vector2 dir = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)).normalized;
                float speed = UnityEngine.Random.Range(projectileSpeed * 0.7f, projectileSpeed * 1.2f);

                Vector2 velocity = dir * speed;

                GameObject currProj = Instantiate(projectilePrefab, firePos, Quaternion .identity);
                EnemyProjectile proj = currProj.GetComponent<EnemyProjectile>();

                if (proj != null)
                {
                    proj.InitProjecitle(
                            controller,
                            controller.Status.attack * damageMultiply,
                            velocity,
                            1.0f,
                            isSpawn ? projectilePrefab : null,
                            isSpawn,
                            lifetime
                        );
                }
                count++;
                yield return new WaitForSeconds(projectileTurm);
            }
        }


        [Obsolete("제거 예정 함수")]
        private void DEP_NormalAttack(EnemyController controller)
        {
            // null 할당 오류 return
            if (controller.Player == null || projectilePrefab == null) return;
            if (controller.FSM.projectileFiredCount >= projectileAmount) return;

            Vector3 playerDelta = controller.Player.position - controller.transform.position;
            if (playerDelta.x != 0)
            {
                controller.SpriteRenderer.flipX = playerDelta.x > 0;
            }

            controller.FSM.fireRoutine = controller.StartCoroutine(DEP_FireProjectiles(controller));
        }

        [Obsolete("제거 예정 함수")]
        private IEnumerator DEP_FireProjectiles(EnemyController controller)
        {
            int count = 0;
            while (count < projectileAmount)
            {
                // 발사
                Vector3 firePos = controller.transform.position + firePointOffset;
                Vector3 dir = (controller.Player.position - firePos).normalized;
                Vector2 velocity = dir * projectileSpeed;
                GameObject currProj = Instantiate(projectilePrefab, firePos, Quaternion.identity, controller.transform);
                EnemyProjectile proj = currProj.GetComponent<EnemyProjectile>();

                if (proj != null)
                {
                    //proj.InitProjecitle(
                    //        controller,
                    //        controller.Status.attack * damageMultiply,
                    //        velocity
                    //    );
                }

                //var rb = currProj.GetComponent<Rigidbody2D>();
                //if (rb != null) rb.linearVelocity = dir * projectileSpeed;
                //Destroy(currProj, 1f);

                count++;
                yield return new WaitForSeconds(projectileTurm);
            }
        }
    }
}
