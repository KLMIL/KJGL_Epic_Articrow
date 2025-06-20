using TMPro;
using UnityEngine;
using YSJ;

namespace Game.Enemy
{
    public class EnemyTakeDamage : MonoBehaviour, IDamagable
    {
        EnemyController ownerController;

        private void Start()
        {
            ownerController = GetComponentInParent<EnemyController>();
        }

        public void TakeDamage(float damage)
        {
            TextMeshPro damageText = Managers.TestPool.Get<TextMeshPro>(Define.PoolID.DamageText);
            damageText.text = damage.ToString();
            damageText.transform.position = transform.position;

            ownerController.FSM.isDamaged = true;
            ownerController.Status.healthPoint -= damage;
            //ownerController.FSM.pendingDamage += damage;

            // TODO: TakeDamage에서 공격자 Transfrom 전달하기
            ownerController.Attacker = ownerController.Player;
        }
    }
}
