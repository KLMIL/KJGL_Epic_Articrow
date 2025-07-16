using UnityEngine;

namespace BMC
{
    // 보스가 데미지 받는 HurtBox
    public class BossHurtBox : MonoBehaviour, IDamagable
    {
        BossFSM _fsm;
        IDamagable _damagable;

        public void Init(BossFSM fsm)
        {
            _fsm = fsm;
            _damagable = _fsm.GetComponent<IDamagable>();
        }

        public void TakeDamage(float damage, Define.EnemyName attacker = Define.EnemyName.None)
        {
            Debug.Log("보스 데미지 주기");
            _damagable.TakeDamage(damage);
        }
    }
}