using UnityEngine;

namespace BMC
{
    public class EnemyStatus : MonoBehaviour, IDamagable
    {
        public bool IsDead { get; private set; }

        public float MaxHP { get; private set; } = 100f;
        public float HP { get; private set; }
        public float MoveSpeed { get; private set; }
        public float RushForce { get; private set; }

        public void Init()
        {
            IsDead = false;
            HP = MaxHP;
            MoveSpeed = 5f;
            RushForce = 10f;
        }

        public void TakeDamage(float damage)
        {
            if(IsDead) 
                return;

            HP = Mathf.Clamp(HP - damage, 0, MaxHP);
            if(HP <=0)
            {
                Die();
            }
        }

        void Die()
        {
            IsDead = true;
            // 사망 애니메이션

            // gameObject.SetActive(false);
            // Destroy(gameObject);
        }
    }
}