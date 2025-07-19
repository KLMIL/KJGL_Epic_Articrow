using BMC;
using System;
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
        float _gravity;
        [SerializeField] GameObject _spawnPrefab;
        bool _isSpawnMode;
        float _lifetime;
        ProjectileDebuff _debuff;

        Rigidbody2D _rb;

        public string targetTag = "PlayerHurtBox";

        public event Action OnDeath;


        public void InitProjecitle(
                EnemyController controller,
                float damage,
                Vector2 velocity,
                float gravity,
                GameObject spawnPrefab = null,
                bool isSpawnMode = false,
                float lifetime = 1.0f,
                ProjectileDebuff debuff = ProjectileDebuff.Normal
            )
        {
            _ownerController = controller;
            _damage = damage;
            _velocity = velocity;
            _gravity = gravity;
            //_spawnPrefab = spawnPrefab;
            _isSpawnMode = isSpawnMode;
            _lifetime = lifetime;
            _debuff = debuff;
            _rb = GetComponent<Rigidbody2D>();
            if (_rb != null)
            {
                _rb.linearVelocity = _velocity;
                _rb.gravityScale = _gravity;
            }
            // 이동 방향으로 회전
            float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            // 생성될 때, Event 추가
            if (isSpawnMode)
            {
                StageManager.Instance.CurrentRoom.GetComponent<NormalRoom>().EnrollEnemy(this);
            }

            Destroy(gameObject, _lifetime);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if ((1 << collision.gameObject.layer) == LayerMask.GetMask("PlayerHurtBox"))
            {
                if (!_isSpawnMode)
                {
                    Transform target = collision.transform;
                    _ownerController.DealDamageToPlayer(_ownerController.Status.attack, target);
                    Destroy(gameObject);
                }

                if (_debuff == ProjectileDebuff.Immobilize)
                {
                    collision.gameObject.GetComponentInParent<PlayerDebuff>().ApplyDebuff(DebuffType.Stun, 1f);
                    // TODO: 이동불가 디버프 효과 부여
                }
            }
            else if ((LayerMask.GetMask("Obstacle") & (1 << collision.gameObject.layer)) != 0)
            {
                Destroy(gameObject);
            }
        }

        private void OnDestroy()
        {
            Debug.LogWarning("Destroy Called");
            if (_isSpawnMode && _spawnPrefab != null)
            {
                Debug.LogWarning("Spawned Enemy");
                Instantiate(_spawnPrefab, transform.position, Quaternion.identity);
                OnDeath?.Invoke();
            }
        }
    }
}
