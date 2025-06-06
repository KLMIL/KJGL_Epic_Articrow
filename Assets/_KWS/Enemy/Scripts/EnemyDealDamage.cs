using UnityEngine;

public class EnemyDealDamage : MonoBehaviour
{
    EnemyController ownerController;
    public string targetTag = "Player";

    private void Start()
    {
        ownerController = GetComponentInParent<EnemyController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(targetTag))
        {
            ownerController.isContactDamageActive = true;

            // 접촉 공격 수행
            if (Time.time - ownerController.lastContactAttackTime >= ownerController.contactAttackCooldown)
            {
                ownerController.DealDamageToPlayer(ownerController.Status.attack);
                ownerController.lastContactAttackTime = Time.time;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (Time.time - ownerController.lastContactAttackTime >= ownerController.contactAttackCooldown)
        {
            ownerController.DealDamageToPlayer(ownerController.Status.attack);
            ownerController.lastContactAttackTime = Time.time;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(targetTag))
        {
            ownerController.isContactDamageActive = false;
        }
    }
}
