using Game.Enemy;
using System.Collections;
using UnityEngine;

/*
 * 지정된 시간 뒤 개체 속도를 원래대로 돌려놓는 코루틴을 실행할 개체
 */

namespace CKT
{
    public class HitSlow : MonoBehaviour
    {
        float _duration;
        float _multiply;
        Collider2D _target;
        float _originSpeed;
        EnemyController _enemyController;

        public void StartSlowCoroutine(float duration, float multiply, Collider2D target)
        {
            _duration = duration;
            _multiply = multiply;
            _target = target;

            _enemyController = _target.GetComponent<EnemyController>();
            if (_enemyController == null)
            {
                Destroy(gameObject);
                return;
            }

            _originSpeed = _enemyController.Status.moveSpeed;
            _enemyController.Status.moveSpeed = _originSpeed * _multiply;

            StartCoroutine(SlowCoroutine());
        }

        private IEnumerator SlowCoroutine()
        {
            yield return new WaitForSeconds(_duration);

            if (_enemyController != null && _enemyController.Status != null)
            {
                if (_enemyController.Status.moveSpeed < _originSpeed)
                {
                    _enemyController.Status.moveSpeed = _originSpeed;
                }
            }

            Destroy(gameObject);
        }
    }
}
