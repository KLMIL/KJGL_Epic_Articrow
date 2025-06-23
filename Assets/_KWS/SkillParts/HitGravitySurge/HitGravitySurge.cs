using Game.Enemy;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CKT
{
    public class HitGravitySurge : MonoBehaviour
    {
        float _baseGravity = 20f;
        float _totalGravity;
        float _minDistance = 0.2f;
        float _duration = 0.5f;
        float _totalDuration;

        List<Coroutine> CoroutineList;

        public void Init(int level)
        {
            _totalGravity = _baseGravity;
            _totalDuration = _duration * level;
        }


        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.isTrigger) return;

            Transform target = collision.transform;
            Game.Enemy.TestEnemyTakeDamage damagable = target.GetComponent<Game.Enemy.TestEnemyTakeDamage>();
            Rigidbody2D rb = target.GetComponentInParent<Rigidbody2D>();
            Game.Enemy.EnemyController enemyController = collision.GetComponentInParent<Game.Enemy.EnemyController>();

            //Debug.LogError($"타겟: {target.gameObject.name}, damagable: {damagable == null}, rb: {rb == null}");

            // 몬스터는 모두 IDamagable을 가지고 있음
            if (damagable != null && rb != null)
            {
                //Debug.LogError("여기 진입?");
                //damagable.TakeDamage(1);
                Vector2 pullTargetPos = transform.position;
                //Debug.LogError("코루틴 시작");

                if (enemyController.IsGravitySurgeCoroutineGO())
                {
                    enemyController.EndGravitySurgeCoroutine();
                }
                enemyController.StartGravitySurgeCoroutine(StartCoroutine(PullCoroutine(rb, pullTargetPos, enemyController)));
            }
        }

        private IEnumerator PullCoroutine(Rigidbody2D rb, Vector2 pullTargetPos, Game.Enemy.EnemyController enemyController)
        {
            float elapsed = 0f;

            rb.gameObject.GetComponent<EnemyController>().FSM.isGravitySurge = true;
            while (elapsed < _totalDuration)
            {
                Vector2 dir = (pullTargetPos - rb.position);
                float distance = dir.magnitude;
                if (distance < _minDistance) break;

                float moveRatio = _totalGravity * Time.fixedDeltaTime / distance;
                moveRatio = Mathf.Clamp(moveRatio, 0.2f, 1.0f);

                Vector2 nextPos = Vector2.Lerp(rb.position, pullTargetPos, moveRatio);
                rb.MovePosition(nextPos);

                elapsed += Time.fixedDeltaTime;
                yield return new WaitForFixedUpdate();
            }

            //Debug.LogError("코루틴 끝");
            rb.gameObject.GetComponent<EnemyController>().FSM.isGravitySurge = false;
            enemyController.EndGravitySurgeCoroutine();
        }
    }
}
