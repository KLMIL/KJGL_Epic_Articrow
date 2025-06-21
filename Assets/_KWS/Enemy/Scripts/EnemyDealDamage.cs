using BMC;
using UnityEngine;
using YSJ;

namespace Game.Enemy
{
    public class EnemyDealDamage : MonoBehaviour
    {
        EnemyController ownerController;
        //public string targetTag = "PlayerHurtBox";

        bool isPlayerContact = false;

        private void Start()
        {
            ownerController = GetComponentInParent<EnemyController>();
        }


        private void OnTriggerEnter2D(Collider2D collision)
        {
            //if (collision.GetComponent<PlayerHurtBox>() == null) return;

            if ((1 << collision.gameObject.layer) == LayerMask.GetMask("PlayerHurtBox"))
            {
                //ownerController.Player = collision.transform;

                if (!ownerController.FSM.isRushAttacked) // 돌진 공격 수행
                {
                    Transform target = collision.transform;
                    ownerController.DealDamageToPlayer(ownerController.Status.attack * ownerController.FSM.rushDamageMultuply, target);
                    ownerController.FSM.isRushAttacked = true;

                    // 임시로 접촉 공격 쿨타임도 돌도록 처리
                    ownerController.FSM.lastContactAttackTime = Time.time;

                    ownerController.FSM.isRushAttacked = true;
                    return;
                }

                isPlayerContact = true;
                ownerController.FSM.isContactDamageActive = true;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if ((1 << collision.gameObject.layer) == LayerMask.GetMask("PlayerHurtBox"))
            {
                isPlayerContact = false;
                ownerController.FSM.isContactDamageActive = false;
            }
        }


        public void DealDamageToPlayer(float damage, Transform targetTransform, bool forceToNextState)
        {
            //IDamagable target = ownerController.Player.GetComponent<IDamagable>();
            //IDamagable target = targetTransform.GetComponent<IDamagable>();

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