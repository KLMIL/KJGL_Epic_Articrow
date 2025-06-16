using UnityEngine;

/*
 * 적 발사체 기능을 정의하는 함수. 발사체 컴포넌트로 할당.
 */

namespace Game.Enemy
{
    public class EnemyProjectile : MonoBehaviour
    {
        EnemyController _ownerController;
        public string targetTag = "Player";

        Rigidbody2D _rb;

        // Projectile Info
        float _damage;
        Vector2 _velocity;
        GameObject _spawnPrefab;


        private void Update()
        {
            
        }


        public void InitProjecitle(
                EnemyController controller,
                float damage,
                Vector2 velocity,
                GameObject spawnPrefab = null
            )
        {
            _ownerController = controller;
            _damage = damage;
            _velocity = velocity;
            _spawnPrefab = spawnPrefab;
            _rb = GetComponent<Rigidbody2D>();
            if (_rb != null)
            {
                _rb.linearVelocity = _velocity;
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag(targetTag) && collision.isTrigger)
            {
                 _ownerController.DealDamageToPlayer(_ownerController.Status.attack);
                Destroy(gameObject);
            }
        }
    }
}
