using UnityEngine;

namespace BMC
{
    public class EnemyHurtBox : MonoBehaviour, IDamagable
    {
        EnemyFSM _fsm;
        IDamagable _damagable;

        public void Init(EnemyFSM fsm)
        {
            _fsm = fsm;
            _damagable = _fsm.GetComponent<IDamagable>();
        }

        public void TakeDamage(float damage)
        {
            Debug.Log("몬스터 데미지 주기");
            _damagable.TakeDamage(damage);
        }
    }
}