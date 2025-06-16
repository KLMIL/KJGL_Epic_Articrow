using UnityEngine;

namespace Game.Enemy
{
    public class EnemyDealDamage : MonoBehaviour
    {
        EnemyController ownerController;
        public string targetTag = "Player";

        bool isPlayerContact = false;

        private void Start()
        {
            ownerController = GetComponentInParent<EnemyController>();
        }


        private void OnTriggerEnter2D(Collider2D collision)
        {
            Debug.LogWarning("Player contact with enemy");
            if (collision.CompareTag(targetTag) && collision.isTrigger)
            {
                if (!ownerController.FSM.isRushAttacked) // 돌진 공격 수행
                {
                    Debug.LogWarning("Rush Attack go");
                    ownerController.DealDamageToPlayer(ownerController.Status.attack * ownerController.FSM.rushDamageMultuply);
                    ownerController.FSM.isRushAttacked = true;

                    // 임시로 접촉 공격 쿨타임도 돌도록 처리
                    ownerController.FSM.lastContactAttackTime = Time.time;
                    return;
                }

                isPlayerContact = true;
                ownerController.FSM.isContactDamageActive = true;
            }

            // 임시로 Rush를 멈출 코드
            if (ownerController.FSM.isRushAttacked == false)
            {
                ownerController.FSM.isRushAttacked = true;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag(targetTag))
            {
                isPlayerContact = false;
                ownerController.FSM.isContactDamageActive = false;
            }
        }


        public void DealDamageToPlayer(float damage, bool forceToNextState)
        {
            IDamagable target = ownerController.Player.GetComponent<IDamagable>();
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