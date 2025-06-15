using UnityEngine;

/*
 * 적 발사체 기능을 정의하는 함수. 발사체 컴포넌트로 할당.
 */

namespace Game.Enemy
{
    public class EnemyProjectile : MonoBehaviour
    {
        EnemyController _ownerController;

        // Projectile Info
        float _damage;
        Vector2 _velocity;
        GameObject _spawnPrefab;
        bool _isSpawnMode;

        Rigidbody2D _rb;

        public string targetTag = "Player";


        private void Update()
        {
            
        }


        public void InitProjecitle(
                EnemyController controller,
                float damage,
                Vector2 velocity,
                GameObject spawnPrefab = null,
                bool isSpawnMode = false
            )
        {
            _ownerController = controller;
            _damage = damage;
            _velocity = velocity;
            _spawnPrefab = spawnPrefab;
            _isSpawnMode = isSpawnMode;
            _rb = GetComponent<Rigidbody2D>();
            if (_rb != null)
            {
                _rb.linearVelocity = _velocity;
            }

            Destroy(gameObject, 1f);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag(targetTag) && collision.isTrigger)
            {
                if (!_isSpawnMode)
                {
                    _ownerController.DealDamageToPlayer(_ownerController.Status.attack);
                    Destroy(gameObject);
                }

            }
        }

        private void OnDestroy()
        {
            if (_isSpawnMode && _spawnPrefab != null)
            {
                Instantiate(_spawnPrefab, transform.position, Quaternion.identity);
            }
        }
    }
}
