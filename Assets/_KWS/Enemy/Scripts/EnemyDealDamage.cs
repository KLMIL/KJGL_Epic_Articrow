using BMC;
using UnityEngine;
using YSJ;

namespace Game.Enemy
{
    public class EnemyDealDamage : MonoBehaviour
    {
        EnemyController ownerController;

        private void Start()
        {
            ownerController = GetComponentInParent<EnemyController>();
        }


        private void OnTriggerEnter2D(Collider2D collision)
        {
            if ((1 << collision.gameObject.layer) == LayerMask.GetMask("PlayerHurtBox"))
            {
                // 돌진 공격 수행
                if (!ownerController.FSM.isRushAttacked) 
                {
                    Transform target = collision.transform;
                    ownerController.DealDamageToPlayer(ownerController.Status.attack * ownerController.FSM.rushDamageMultuply, target);
                    ownerController.FSM.isRushAttacked = true;
                    ownerController.FSM.lastContactAttackTime = Time.time;
                    return;
                }

                ownerController.FSM.isContactDamageActive = true;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if ((1 << collision.gameObject.layer) == LayerMask.GetMask("PlayerHurtBox"))
            {
                ownerController.FSM.isContactDamageActive = false;
            }
        }


        public void DealDamageToPlayer(float damage, Transform targetTransform, bool forceToNextState)
        {
            PlayerHurtBox target = targetTransform.GetComponent<PlayerHurtBox>();

            if (target != null)
            {
                target.TakeDamage(damage);

                if (forceToNextState)
                {
                    ownerController.ForceToNextState();
                }
            }
        }
    }
}