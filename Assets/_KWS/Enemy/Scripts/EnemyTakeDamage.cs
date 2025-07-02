using TMPro;
using UnityEngine;
using BMC;

namespace Game.Enemy
{
    public class EnemyTakeDamage : MonoBehaviour, IDamagable
    {
        EnemyController ownerController;
        ShowDamageText showDamageText;

        //TextMeshPro damageText;

        private void Awake()
        {
            showDamageText = GetComponent<ShowDamageText>();
        }

        private void Start()
        {
            ownerController = GetComponentInParent<EnemyController>();
            GetComponentInChildren<TestEnemyTakeDamage>().Init(this);
        }

        private void OnDisable()
        {
            //damageText.text = "0";
            //damageText = null;
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
            }

            //// 대미지 부여 텍스트
            //if (damageText != null)
            //{
            //    if (damageText.gameObject.activeInHierarchy)
            //    {
            //        damageText.text = (float.Parse(damageText.text) + currDamage).ToString("F0");

            //        Color color = damageText.color;
            //        color.a = 1;
            //        damageText.color = color;
            //    }
            //}
            //else
            //{
            //    damageText = Managers.TestPool.Get<TextMeshPro>(Define.PoolID.DamageText);
            //    damageText.text = currDamage.ToString("F0");
            //}
            //damageText.transform.position = this.transform.position + this.transform.up;
            showDamageText.Show(currDamage);


            ownerController.Status.healthPoint -= currDamage;

            // TODO: 반납하면 Jar Larva 안 죽는 문제 있어서 임시 주석
            //if (ownerController.Status.healthPoint <= 0)
            //{
            //    Managers.TestPool.Return(Define.PoolID.DamageText, gameObject);
            //}


            //ownerController.FSM.pendingDamage += damage;

            // TODO: TakeDamage에서 공격자 Transfrom 전달하기
            ownerController.Attacker = ownerController.Player;
        }
    }
}
