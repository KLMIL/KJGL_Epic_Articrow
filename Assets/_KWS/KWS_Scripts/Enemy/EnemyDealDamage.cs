using UnityEngine;

public class EnemyDealDamage : MonoBehaviour
{
    EnemyController ownerController;
    public string targetTag = "Player";

    public float attackCooldown;
    private float lastAttackTime = -Mathf.Infinity;

    private void Awake()
    {
        ownerController = GetComponent<EnemyController>();
        attackCooldown = ownerController.Status.attackCooldown;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(targetTag))
        {
            var damagable = other.GetComponent<IDamagable>();
            damagable.TakeDamage(ownerController.Status.attack);
            lastAttackTime = Time.time;

            // 투사체의 경우 파괴
            //if (gameObject.CompareTag("Projectile")) Destroy(gameObject);
        }
    }

    // 플레이어가 계속 붙어있다면 공격속도 시간마다 타격
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag(targetTag))
        {
            if (Time.time - lastAttackTime >= attackCooldown)
            {
                var damagable = collision.GetComponent<IDamagable>();
                if (damagable != null)
                {
                    damagable.TakeDamage(ownerController.Status.attack);
                    lastAttackTime = Time.time;
                }
            }
        }
    }

    // 플레이어가 떨어졌다 다시 붙으면 즉시 타격 -> 제거해도 됨
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(targetTag))
        {
            lastAttackTime = -Mathf.Infinity;
        }
    }


    //// 플레이어가 적에 접촉했을 때 대미지 부여. 지속적으로 닿아 있다면 대미지 부여
    //private void OnTriggerStay(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        TryAttackPlayer(other.gameObject);
    //    }
    //}

    //private void TryAttackPlayer(GameObject playerObj)
    //{
    //    if (Time.time - _lastAttackTime < Status.attackCooldown) return;

    //    var player = playerObj.GetComponent<PlayerController>(); // 플레이어 오브젝트의 TakeDamage 스크립트 호출

    //    if (player != null)
    //    {
    //        //player.TakeDamage(Status.attack); // 플레이어의 피격 함수 호출
    //        _lastAttackTime = Time.time;
    //    }
    //}
}
