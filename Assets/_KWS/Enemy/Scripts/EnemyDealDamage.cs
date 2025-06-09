using System.Collections.Generic;
using UnityEngine;

public class EnemyDealDamage : MonoBehaviour
{
    EnemyController ownerController;
    public string targetTag = "Player";

    bool isPlayerContact = false;

    private void Start()
    {
        ownerController = GetComponentInParent<EnemyController>();
    }

    private void Update()
    {
        if (!isPlayerContact) return;

        // 접촉 공격 수행
        if (Time.time - ownerController.lastContactAttackTime >= ownerController.contactAttackCooldown && ownerController.CurrentStateName != "Die")
        {
            ownerController.DealDamageToPlayer(ownerController.Status.attack);
            ownerController.lastContactAttackTime = Time.time;
        }


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(targetTag))
        {
            if (gameObject.name.Contains("SporeSlimeProjectile"))
            {
                ownerController.DealDamageToPlayer(ownerController.Status.attack);
                Destroy(gameObject); // 투사체 파괴
            }
            else if (!ownerController.isRushAttacked) // 돌진 공격 수행
            {
                ownerController.DealDamageToPlayer(ownerController.Status.attack * ownerController.rushDamageMultuply);
                ownerController.isRushAttacked = true;

                // 임시로 접촉 공격 쿨타임도 돌도록 처리
                ownerController.lastContactAttackTime = Time.time;
                return;
            }

            isPlayerContact = true;
            ownerController.isContactDamageActive = true;
        }

        // 임시로 Rush를 멈출 코드
        if (ownerController.isRushAttacked == false)
        {
            ownerController.isRushAttacked = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(targetTag))
        {
            isPlayerContact = false;
            ownerController.isContactDamageActive = false;
        }
    }
}
