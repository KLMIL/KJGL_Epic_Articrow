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

        public void TakeDamage(float damage, Transform attacker = null)
        {
            if (ownerController.FSM.isDied) return;

            if (!ownerController.FSM.isSuperArmor)
            {
                ownerController.FSM.isDamaged = true;
            }
            float currDamage = damage;

            showDamageText.Show(currDamage);


            ownerController.Status.healthPoint -= currDamage;

            ownerController.Attacker = ownerController.Player;
        }
    }
}
