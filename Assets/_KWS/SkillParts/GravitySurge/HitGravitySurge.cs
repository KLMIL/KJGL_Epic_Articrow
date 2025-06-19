using System.Collections;
using UnityEngine;

namespace CKT
{
    public class HitGravitySurge : MonoBehaviour
    {
        float _baseGravity = 20f;
        float _totalGravity;
        float _minDistance = 0.2f;
        float _duration = 0.5f;

        public void Init(int level)
        {
            _totalGravity = _baseGravity * level;
        }


        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.isTrigger) return;

            Transform target = collision.transform;
            IDamagable damagable = target.GetComponent<IDamagable>();
            Rigidbody2D rb = target.GetComponent<Rigidbody2D>();

            // 몬스터는 모두 IDamagable을 가지고 있음
            if (damagable != null && rb != null)
            {
                Vector2 pullTargetPos = transform.position;
                StartCoroutine(PullCoroutine(rb, pullTargetPos));

            }
        }

        private IEnumerator PullCoroutine(Rigidbody2D rb, Vector2 pullTargetPos)
        {
            float elapsed = 0f;
            while (elapsed < _duration)
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
        }
    }
}
