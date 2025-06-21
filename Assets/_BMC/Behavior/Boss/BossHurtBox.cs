using UnityEngine;

namespace BMC
{
    public class BossHurtBox : MonoBehaviour, IDamagable
    {
        BossFSM _fsm;

        public void Init(BossFSM fsm)
        {
            _fsm = fsm;
        }

        public void TakeDamage(float damage)
        {
            Debug.Log("보스 데미지 주기");
            _fsm.Status.TakeDamage(damage);
        }
    }
}