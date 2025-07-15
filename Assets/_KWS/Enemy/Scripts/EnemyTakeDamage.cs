using TMPro;
using UnityEngine;
using BMC;

namespace Game.Enemy
{
    public class EnemyTakeDamage : MonoBehaviour, IDamagable
    {
        EnemyController ownerController;
        ShowDamageText showDamageText;

        private void Awake()
        {
            showDamageText = GetComponent<ShowDamageText>();
        }

        private void Start()
        {
            ownerController = GetComponentInParent<EnemyController>();
        }

        public void TakeDamage(float damage)
        {
            if (ownerController.FSM.isDied) return;

            if (!ownerController.FSM.isSuperArmor)
            {
                ownerController.FSM.isDamaged = true;
            }
            float currDamage = damage;

            // 표식이 있는지 검사해서, 있다면 대미지 배율 적용
            bool isMarked = Time.time < ownerController.FSM.enemyDamagedMultiplyRemainTime;
            if (isMarked)
            {
                currDamage *= ownerController.FSM.enemyDamagedMultiply;
                currDamage = 0.01f * Mathf.RoundToInt(currDamage * 100f);
            }

            showDamageText.Show(currDamage);


            ownerController.Status.healthPoint -= currDamage;

            ownerController.Attacker = ownerController.Player;
        }
    }
}
