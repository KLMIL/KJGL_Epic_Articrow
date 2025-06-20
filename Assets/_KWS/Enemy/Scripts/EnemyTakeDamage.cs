using TMPro;
using UnityEngine;
using YSJ;

namespace Game.Enemy
{
    public class EnemyTakeDamage : MonoBehaviour, IDamagable
    {
        EnemyController ownerController;
        TextMeshPro damageText;

        private void Start()
        {
            ownerController = GetComponentInParent<EnemyController>();
        }

        private void OnDisable()
        {
            damageText = null;
        }

        public void TakeDamage(float damage)
        {
            ownerController.FSM.isDamaged = true;
            float currDamage = damage;

            // 표식이 있는지 검사해서, 있다면 대미지 배율 적용
            bool isMarked = Time.time < ownerController.FSM.enemyDamagedMultiplyRemainTime;
            if (isMarked)
            {
                currDamage *= ownerController.FSM.enemyDamagedMultiply;
            }

            // 대미지 부여 텍스트
            if (damageText != null && damageText.gameObject.activeInHierarchy)
            {
                damageText.text = (float.Parse(damageText.text) + currDamage).ToString();

                Color color = damageText.color;
                color.a = 1;
                damageText.color = color;
            }
            else
            {
                damageText = Managers.TestPool.Get<TextMeshPro>(Define.PoolID.DamageText);
                damageText.text = currDamage.ToString();
            }
            damageText.transform.position = this.transform.position + this.transform.up;


            ownerController.Status.healthPoint -= currDamage;
            //ownerController.FSM.pendingDamage += damage;

            // TODO: TakeDamage에서 공격자 Transfrom 전달하기
            ownerController.Attacker = ownerController.Player;
        }
    }
}
