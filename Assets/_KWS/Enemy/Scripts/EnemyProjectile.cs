using UnityEngine;
using UnityEngine.Rendering;

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
        float _gravity;
        [SerializeField] GameObject _spawnPrefab;
        bool _isSpawnMode;
        float _lifetime;

        Rigidbody2D _rb;

        public string targetTag = "Player";


        public void InitProjecitle(
                EnemyController controller,
                float damage,
                Vector2 velocity,
                float gravity,
                GameObject spawnPrefab = null,
                bool isSpawnMode = false,
                float lifetime = 1.0f
            )
        {
            _ownerController = controller;
            _damage = damage;
            _velocity = velocity;
            _gravity = gravity;
            //_spawnPrefab = spawnPrefab;
            _isSpawnMode = isSpawnMode;
            _lifetime = lifetime;
            _rb = GetComponent<Rigidbody2D>();
            if (_rb != null)
            {
                _rb.linearVelocity = _velocity;
                _rb.gravityScale = _gravity;
            }

            Destroy(gameObject, _lifetime);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag(targetTag) && collision.isTrigger)
            {
                if (!_isSpawnMode)
                {
                    Transform target = collision.transform;
                    _ownerController.DealDamageToPlayer(_ownerController.Status.attack, target);
                    Destroy(gameObject);
                }

            }
        }

        private void OnDestroy()
        {
            Debug.LogWarning("Destroy Called");
            if (_isSpawnMode && _spawnPrefab != null)
            {
                Debug.LogWarning("Spawned Enemy");
                Instantiate(_spawnPrefab, transform.position, Quaternion.identity);
            }
        }
    }
}
