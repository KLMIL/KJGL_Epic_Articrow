using UnityEngine;

public class EnemyDealDamage : MonoBehaviour
{
    EnemyController ownerController;
    public string targetTag = "Player";

    private void Awake()
    {
        ownerController = GetComponent<EnemyController>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(targetTag))
        {
            var damagable = other.GetComponent<IDamagable>();
            damagable.TakeDamage(ownerController.Status.attack);

            // 투사체의 경우 파괴
            //if (gameObject.CompareTag("Projectile")) Destroy(gameObject);
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
